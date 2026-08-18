#!/bin/bash
# WinForms 코드 전용 문법/타입 체크 스크립트 (임시 개발 보조용, 이 저장소의 정식 빌드 산출물이 아님)
#
# 이 리포지토리는 Windows용 .NET WinForms 앱(net8.0-windows, UseWindowsForms=true)이라
# 실제 빌드/실행은 Visual Studio 2022(Windows)에서 해야 한다. Linux 샌드박스의 .NET SDK에는
# Microsoft.NET.Sdk.WindowsDesktop 빌드 타깃이 빠져 있어 `dotnet build`가 아예 동작하지 않으므로,
# 대신 NuGet의 Microsoft.WindowsDesktop.App.Ref(WinForms 참조 어셈블리)를 받아 csc(Roslyn)로
# 직접 컴파일해 문법/타입 오류만 미리 잡아내는 용도다. 리소스 임베딩, 아이콘, app.manifest 등은
# 다루지 않으므로 이 스크립트 통과가 "정식 빌드 성공"을 보장하지 않는다 — 최종 확인은 VS2022에서.
set -e

WORK=/tmp/winref
REFS="$WORK/refs"

# .NET SDK가 없으면 설치 (Roslyn csc.dll이 SDK 안에 포함되어 있음)
if ! command -v dotnet >/dev/null 2>&1; then
  echo "[compile_check] dotnet SDK가 없어 설치합니다..."
  sudo apt-get update -qq
  sudo apt-get install -y -qq dotnet-sdk-8.0
fi

CSC=$(find /usr/lib/dotnet/sdk /usr/share/dotnet/sdk "$HOME/.dotnet/sdk" -maxdepth 5 -iname "csc.dll" -path "*Roslyn*" 2>/dev/null | head -1)
if [ -z "$CSC" ]; then
  echo "[compile_check] csc.dll을 찾지 못했습니다 (.NET SDK 설치 확인 필요)." >&2
  exit 1
fi

# WinForms 참조 어셈블리(Microsoft.WindowsDesktop.App.Ref) + NuGet 패키지(Microsoft.Data.SqlClient, ClosedXML)를
# 처음 실행 시에만 내려받아 캐시한다.
if [ ! -d "$REFS" ] || [ -z "$(ls -A "$REFS" 2>/dev/null)" ]; then
  echo "[compile_check] 참조 어셈블리를 준비합니다 (최초 1회, 인터넷 필요)..."
  mkdir -p "$WORK" "$REFS"
  cd "$WORK"

  WDVER=8.0.29
  SQLVER=5.2.2
  XLVER=0.104.1

  curl -sS -m 60 -o wdref.nupkg "https://api.nuget.org/v3-flatcontainer/microsoft.windowsdesktop.app.ref/${WDVER}/microsoft.windowsdesktop.app.ref.${WDVER}.nupkg"
  curl -sS -m 60 -o netref.nupkg "https://api.nuget.org/v3-flatcontainer/microsoft.netcore.app.ref/${WDVER}/microsoft.netcore.app.ref.${WDVER}.nupkg"
  curl -sS -m 60 -o sqlclient.nupkg "https://api.nuget.org/v3-flatcontainer/microsoft.data.sqlclient/${SQLVER}/microsoft.data.sqlclient.${SQLVER}.nupkg"
  curl -sS -m 60 -o closedxml.nupkg "https://api.nuget.org/v3-flatcontainer/closedxml/${XLVER}/closedxml.${XLVER}.nupkg"

  mkdir -p _wd _net _sql _xl
  ( cd _wd && unzip -q ../wdref.nupkg )
  ( cd _net && unzip -q ../netref.nupkg )
  ( cd _sql && unzip -q ../sqlclient.nupkg )
  ( cd _xl && unzip -q ../closedxml.nupkg )

  cp _wd/ref/net8.0/*.dll "$REFS"/
  cp _net/ref/net8.0/*.dll "$REFS"/
  cp _sql/ref/net8.0/Microsoft.Data.SqlClient.dll "$REFS"/
  cp _xl/lib/netstandard2.1/ClosedXML.dll "$REFS"/

  rm -rf _wd _net _sql _xl ./*.nupkg

  cat > "$WORK/GlobalUsings.g.cs" << 'EOF'
global using global::System;
global using global::System.Collections.Generic;
global using global::System.IO;
global using global::System.Linq;
global using global::System.Threading;
global using global::System.Threading.Tasks;
global using global::System.Drawing;
global using global::System.Windows.Forms;
EOF
fi

# Guna.UI2.WinForms(사이드바/카드형 UI 컨트롤)는 net8.0-windows 전용 lib가 아직 없어
# net7.0-windows7.0 타깃 DLL을 그대로 참조로 쓴다(NuGet이 net8.0-windows 프로젝트에도
# 이 타깃을 호환 자산으로 선택하는 것과 동일). 위 최초 준비 블록과 별개로, 파일이 없을
# 때만 받는다 — .csproj에 이 패키지가 나중에 추가됐을 수도 있어서.
if [ ! -f "$REFS/Guna.UI2.dll" ]; then
  echo "[compile_check] Guna.UI2.WinForms 참조 어셈블리를 준비합니다..."
  GUNAVER=2.0.4.8
  TMPD=$(mktemp -d)
  curl -sS -m 60 -o "$TMPD/guna.nupkg" "https://api.nuget.org/v3-flatcontainer/guna.ui2.winforms/${GUNAVER}/guna.ui2.winforms.${GUNAVER}.nupkg"
  ( cd "$TMPD" && unzip -q guna.nupkg )
  cp "$TMPD/lib/net7.0-windows7.0/Guna.UI2.dll" "$REFS"/
  rm -rf "$TMPD"
fi

cd "$(dirname "$0")/pStock"

REFARGS=()
for f in "$REFS"/*.dll; do
  REFARGS+=("-reference:$f")
done

FILES=$(find . -iname "*.cs" -not -path "*/obj/*" -not -path "*/bin/*")
FILES="$FILES $WORK/GlobalUsings.g.cs"

dotnet "$CSC" -nologo -target:winexe -langversion:latest -nullable:enable \
  -define:WINDOWS -out:"$WORK/pStock_check.dll" \
  "${REFARGS[@]}" \
  $FILES 2>&1
