#!/bin/bash
# WinForms 코드 전용 문법/타입 체크 스크립트 (임시, 이 저장소 정식 빌드 대상 아님)
# Linux 샌드박스에는 Microsoft.NET.Sdk.WindowsDesktop 빌드 타깃이 없어 `dotnet build`가 안 되므로,
# NuGet의 Microsoft.WindowsDesktop.App.Ref 참조 어셈블리를 받아 csc로 직접 컴파일해 확인한다.
set -e
cd "$(dirname "$0")/pStock"

REFS=/tmp/winref/refs
CSC=/usr/lib/dotnet/sdk/8.0.129/Roslyn/bincore/csc.dll

REFARGS=()
for f in "$REFS"/*.dll; do
  REFARGS+=("-reference:$f")
done

FILES=$(find . -iname "*.cs" -not -path "*/obj/*" -not -path "*/bin/*")
FILES="$FILES /tmp/winref/GlobalUsings.g.cs"

dotnet "$CSC" -nologo -target:winexe -langversion:latest -nullable:enable \
  -define:WINDOWS -out:/tmp/winref/pStock_check.dll \
  "${REFARGS[@]}" \
  $FILES 2>&1
