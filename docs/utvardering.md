# utvardering.md

## Deltagare

- Henrik Nyström
- Desirée Skönneberg
- Erik Holgersson
- Kalle Lindberg

---

# 1. Vad vi byggde

Projektet började med en brainstorming-session den 14:e maj där gruppen diskuterade flera olika projektidéer.

Några av förslagen var:

- En enkel och gratis mobilapp för att lära sig glosor
- En sportfiskeapp där väder, vind och transportsträckor kunde beräknas för olika fiskeplatser
- En webbapp som varnar för nattfrost för hobbyodlare

Gruppen valde det sista alternativet och utvecklade projektet **Nattfrost**.

Nattfrost är en webbapplikation där användaren registrerar sin e-postadress och stad. Systemet hämtar väderdata varje kväll och analyserar om temperaturen riskerar att sjunka under en definierad gräns för nattfrost. Om risk upptäcks skickas automatiskt ett varningsmail till användaren.

Applikationen riktar sig främst till hobbyodlare som behöver skydda växter eller grödor från köldskador under natten eller morgonen.

Mellan brainstorming-sessionerna genomfördes även research kring olika tekniska lösningar, bland annat:

- SMTP och e-postutskick
- Geolokalisering
- Väderprognoser och API:er

Gruppen valde att använda OpenMeteoService eftersom tjänsten kunde lösa både geolokalisering och väderprognoser inom samma API-ekosystem.

Det skapades även en logotyp både som vektorgrafik och pixelgrafik för projektet.

---

# 2. Planering och genomförande

Projektet startade den 21:e maj.

Arbetet började med att gruppen skapade en enklare mockup över frontendens utseende samt ett flödesschema över systemets struktur och dataflöde.

Utifrån dessa skapades en backlog med fem user stories som organiserades i Trello.

Gruppen valde att tidigt implementera en vertical slice från frontend till backend och databas för att snabbt få en fungerande grund för systemet.

Eftersom större delen av arbetet utfördes genom mob programming ansågs traditionella daily meetings inte nödvändiga under projektets korta omfattning.

Efter den första vertical slicen skapades separata feature branches för kvarvarande funktionalitet såsom:

- Geolokalisering
- E-postutskick
- Väderintegration

---

# 3. Hur samarbetet fungerade

Samarbetet fungerade över förväntan väl trots att flera gruppmedlemmar tidigare haft ledande roller i andra projekt. Gruppen arbetade snabbt och effektivt både under planering och implementation utan större konflikter kring ansvar eller beslutsfattande.

Det goda samarbetet gjorde att projektet upplevdes som både roligare och mindre stressigt trots de problem som uppstod under utvecklingen.

Under delar av arbetet användes mob programming där rollerna driver och navigator roterades kontinuerligt. Gruppen upplevde detta som mycket lärorikt eftersom samtliga deltagare blev involverade i hela systemflödet istället för enbart isolerade delar av projektet.

Flera gruppmedlemmar hade tidigare erfarenhet av pair programming och branch-baserade arbetsflöden, men detta var första gången gruppen genomförde mob programming över en hel vertical slice från frontend till backend och databas.

---

# 4. Hur vi arbetade med Git

Projektet genomfördes i ett gemensamt GitHub-repository med branch-strukturen:

- main
- dev
- feature branches

Gruppen arbetade kontinuerligt med commits, merges och integrationer under hela projektet.

Stabila milstolpar markerades med Git tags för att tydliggöra fungerande versioner av projektet.

Under projektets gång uppstod även en medveten merge-konflikt då två utvecklare arbetade i samma frontend-kod samtidigt. Gruppen valde att lösa konflikten tillsammans med hjälp av VS Codes inbyggda merge manager, vilket ingen i gruppen tidigare använt.

Konflikten löstes relativt snabbt, men därefter uppstod ett mer komplicerat problem när ett Git-kommando av misstag resulterade i en rebase istället för en vanlig merge mot dev-branchen.

Detta skapade förvirring eftersom gruppen initialt inte förstod varför repositoryts historik förändrades på ett oväntat sätt. Problemet krävde gemensam felsökning och tog relativt lång tid att lösa.

Gruppen fick därmed praktisk erfarenhet av hur känsligt Git kan vara vid felaktiga kommandon och hur viktigt det är med god kommunikation, branch-disciplin och backup-strategier vid gemensam utveckling.

---

# 5. Vad som fungerade bra

Flera delar av projektet fungerade mycket bra.

Den tidiga implementationen av en vertical slice gjorde att gruppen snabbt fick en fungerande grund att bygga vidare på.

Kontinuerlig integration och användningen av feature branches gjorde det möjligt att arbeta parallellt utan att förlora överblick över projektet.

Mob programming upplevdes som särskilt värdefullt eftersom alla gruppmedlemmar fick delta i centrala delar av systemet istället för att enbart arbeta i isolerade delar.

Gruppen upplevde även att Trello fungerade bra för att strukturera user stories och prioriteringar under projektets korta tidsram.

Överlag är gruppen mycket nöjd med både arbetsprocessen och slutresultatet. Det upplevdes särskilt motiverande att se systemet fungera i praktiken när varningsutskicken nådde mobilen i realtid.

---

# 6. Vad vi hade gjort annorlunda

Om projektet genomförts igen hade gruppen arbetat mer strukturerat kring branch-hantering och koordinering av frontend-arbetet för att minska risken för merge-konflikter.

Gruppen hade även velat ha bättre kunskap kring skillnaden mellan merge och rebase innan projektstart för att undvika vissa problem under integrationen.

Flera förbättringar hade även kunnat göras i själva applikationen, exempelvis:

- Möjlighet att avregistrera sig från tjänsten
- Verifiering av e-postadress innan registrering
- Möjlighet för användaren att själv välja temperaturgräns beroende på hur känsliga växterna är

Gruppen hade även kunnat arbeta med fler mindre commits för att ytterligare förenkla felsökning och integration.

---

# 7. Vad vi lärde oss

Projektet gav gruppen mycket praktisk erfarenhet av Git och gemensam utveckling i en gemensam kodbas.

Flera gruppmedlemmar utvecklade sina kunskaper inom olika områden, exempelvis:

- GUI och frontend-design
- Bash och Git-kommandon
- SMTP och e-postutskick i .NET
- Versionshantering och Git tags

Gruppen fick även för första gången praktisk erfarenhet av:

- Mob programming över en hel vertical slice
- Merge-konflikter i större gemensamma koddelar
- VS Codes inbyggda merge manager
- Versionsnummer och release tags

Branch-baserat arbete var däremot redan bekant för större delen av gruppen sedan tidigare projekt.

Projektet gav därför framförallt fördjupad förståelse för integration, konflikthantering och kontinuerligt samarbete i Git.

---

# 8. Slutsats

Överlag är gruppen mycket nöjd med både arbetsprocessen och slutprodukten.

Projektet gav praktisk erfarenhet av verklighetsnära Git-arbete, kontinuerlig integration och samarbete i en gemensam kodbas under tidspress.

Trots vissa tekniska problem upplevdes projektet som mycket lärorikt och gav gruppen större förståelse för både versionshantering och gemensam systemutveckling.
