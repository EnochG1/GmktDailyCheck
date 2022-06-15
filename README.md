# GmktDailyCheck
Gmarket 출석체크 도우미 (C#)
http://promotion.gmarket.co.kr/Event/pluszone.asp 의 출석체크를 해주는 프로그램
빌드 후 생성된 exe 파일을 작업스케쥴러에 넣어주면 매일 출첵이 가능합니다.

# 설정방법
1. 빌드 후 생성되는 config 파일에 본인의 로그인 정보를 입력 (id/pw)
2. PC에 설치되어 있는 Chrome으로 실행하려는 경우, ChromePath를 공란("")으로 수정

## Chromedriver와 Chrome 버전 차이로 인한 이슈 해결방법
Nuget 기준 최신 Chromedriver 버전은 85.x인데, 기본 chrome 버전과 차이가 많이 나고 해당하는 chrome을 찾기 어려울 경우
chromedriver와 chrome 실행파일을 맞춰서 chromedriver가 지정한 chrome을 실행하도록 설정이 가능하다.
1. chromium browser snapshot 페이지에 접속하여 맞는 OS를 선택. https://commondatastorage.googleapis.com/chromium-browser-snapshots/index.html
2. 원하는 빌드 버전을 선택하여 chromedriver와 chrome을 다운로드 받는다.
  ex) https://commondatastorage.googleapis.com/chromium-browser-snapshots/index.html?prefix=Win/1000015/ 에서
  chrome_win.zip과 chromedriver_win32.zip을 다운로드하고, chromedriver_win32.zip의 chromedriver.exe는 빌드된 위치의 chromedriver.exe를 덮어쓰기한다.
  (빌드번호 1000015의 Chrome 버전은 103.0.5046.0에 해당하는데, 각 빌드번호의 Chrome 버전은 다운받아서 chromedriver.exe 실행 후 로그를 체크하거나 버전별 빌드번호 확인.)
3. config ChromePath에 실행할 chrome 파일 위치를 지정
  ex) chrome_win.zip을 C:\chrome_win에 압축 풀었을 경우,  ChromePath = "C:\chrome-win\chrome.exe" 로 입력.
4. 빌드된 GmarketDailyCheck.exe 실행시켜서 해당 브라우저가 실행되는지 확인.
