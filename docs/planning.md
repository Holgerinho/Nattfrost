# planning.md

## Deltagare

- Henrik Nyström
- Desirée Skönneberg
- Erik Holgersson
- Kalle Lindberg

---

# 1. Projektnamn

**Project Nattfrost Webbapp**

---

# 2. Produktidé

Projektet “Nattfrost” är en webbapplikation riktad till hobbyodlare.

Användaren registrerar sin e-postadress och stad i systemet. Varje kväll hämtar systemet väderdata för användarens stad och analyserar om temperaturen riskerar att sjunka under en definierad gräns för nattfrost.

Om risk för nattfrost upptäcks skickas automatiskt en varning via e-post till berörda användare.

Syftet med applikationen är att hjälpa användare skydda växter och odlingar från köldskador.

---

# 3. Teknik och arbetssätt

## Teknik

- Frontend: React
- Backend: ASP.NET Web API
- Databas: SQL LocalDB
- Repository: GitHub

## Verktyg

- Visual Studio
- VS Code
- SQL Server Management Studio (SSMS)
- Trello
- Notepad++

## Arbetsmetod

- XP-inspirerat arbetssätt
- Mob programming
- Kontinuerlig integration
- Feature branches och pull requests

## Testning

På grund av projektets begränsade omfattning genomförs främst enklare funktionella tester och tester av hela lösningen.

## Tidsplanering

- Planering: ~4 h
- Mob programming: ~4–6 h
- Individuellt arbete: ~4–6 h
- Avslutning och rapport: ~2 h

---

# 4. User Stories och backlog

User stories prioriteras i nummerordning där lägre nummer innebär högre prioritet.

Prioriteringen baseras både på projektets tekniska beroenden och på gruppens gemensamma lärande. De viktigaste beståndsdelarna i en modern .NET-webbapplikation prioriteras tidigt så att samtliga gruppmedlemmar aktivt får delta i implementationen av frontend, backend, databas, API-anrop och integrationer.

Detta gjordes för att minimera kunskapsluckor inom gruppen och säkerställa att alla deltagare fick praktisk erfarenhet av de centrala moment som behandlats i den parallellt pågående kursen Webbutveckling.

---

## 1. Registrera användare

**As a user, I want to enter my email address and city, so that I can receive frost warnings.**

### Tasks

- User can enter an email address.
- User can enter a city name.
- The app validates that the email has a valid format.
- The user gets feedback if something is missing or invalid.

---

## 2. Spara användare i databas

**As the system, I want to save user subscriptions in an SQL database, so that users do not need to enter their information every day.**

### Tasks

- Email and city are saved in the SQL database.
- Invalid data is not saved.
- Duplicate subscriptions are prevented or handled.

---

## 3. Hämta väderdata

**As the system, I want to fetch the weather forecast for each saved city every evening, so that I can check the coming night’s temperature.**

### Tasks

- The system retrieves saved subscriptions from the database.
- The system uses the city name to get coordinates/weather data from an API.
- The system checks the forecast for the coming night.
- API errors are handled safely.

---

## 4. Analysera temperatur

**As the system, I want to compare the nightly forecast with a warning threshold, so that I can decide whether the user should be warned.**

### Tasks

- The system has a defined warning threshold, for example 4°C.
- The system finds the lowest expected night temperature.
- The system compares the temperature with the threshold.
- The system only triggers a warning when the forecast is below the threshold.

---

## 5. Skicka varningsmail

**As a user, I want to receive an email when the coming night will be colder than the warning level, so that I can prepare in advance.**

### Tasks

- The system sends an email using SMTP when a warning is triggered.
- The email includes the city.
- The email includes the expected lowest temperature.
- The email is not sent when no warning is needed.

---

# 5. Prioriterad backlog

| Prioritet | Funktion | Status |
|---|---|---|
| Hög | Registrera användare | Ej påbörjad |
| Hög | Spara användare i databas | Ej påbörjad |
| Medium | Hämta väderdata från API | Ej påbörjad |
| Medium | Temperaturanalys | Ej påbörjad |
| Låg | Skicka varningsmail | Ej påbörjad |

---

# 6. Samarbete och Git-strategi

Gruppen arbetar i en gemensam kodbas via GitHub.

Följande branchstruktur används:

- `main`  
  Stabil branch för fungerande och testad kod.

- `dev`  
  Integrationsbranch där features sammanfogas innan merge till `main`.

- `feature/*`  
  Separata feature branches för utveckling av specifik funktionalitet.

All kod som integreras i `main` ska gå via branch och merge.

Stabila versioner av projektet markeras med Git tags för att tydliggöra större milstolpar och fungerande integrationer.

Under delar av arbetet används mob programming där rollerna driver och navigator roteras ungefär var 20:e minut.

Gruppen kommer även medvetet skapa och lösa minst en merge-konflikt tillsammans.
