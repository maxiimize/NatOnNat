# NätOnNät E-Commerce Platform

##  Översikt
NätOnNät är en modern e-handelsplattform byggd med .NET 9 och Clean Architecture-principer. Systemet består av en Web API, en MVC-webbapplikation och en administrativ konsollapplikation.

##  Arkitektur

### Clean Architecture
Projektet följer Clean Architecture med följande lager:

#### **Domain Layer** (`Domain/`)
- Innehåller affärsentiteter och gränssnitt
- Ingen beroende på andra lager
- Implementerar Builder Pattern för Product-entiteten

#### **Infrastructure Layer** (`Infrastructure/`)
- Data Access Layer med Entity Framework Core
- Repository Pattern implementation
- Identity-hantering med ASP.NET Core Identity
- SQL Server databaskoppling

#### **Application Layer** (`Application/`)
- Services och DTOs
- Affärslogik och orchestrering
- HTTPClient-integration för API-anrop

#### **Presentation Layer** 
- **API** (`Api/`): RESTful Web API
- **MVC** (`NätOnNät.web/`): Responsiv webbapplikation
- **Console** (`Console/`): Administrationsverktyg

##  Teknisk Stack

### Backend
- **.NET 9** - Senaste .NET-ramverket
- **ASP.NET Core** - Web framework
- **Entity Framework Core 9** - ORM för databashantering
- **SQL Server** - Databas
- **ASP.NET Core Identity** - Autentisering och auktorisering

### Frontend
- **Bootstrap 5.3** - CSS framework
- **Bootstrap Icons** - Ikonbibliotek
- **Vanilla JavaScript** - Interaktivitet
- **Mobile First Design** - Responsiv design

##  Design Patterns & Principer

### Design Patterns
1. **Builder Pattern** - För att skapa Product-objekt på ett strukturerat sätt
2. **Repository Pattern** - Abstraktion av dataåtkomst
3. **Dependency Injection** - Löst kopplade komponenter
4. **Service Pattern** - Affärslogik separerad från controllers

### SOLID-principer
- **Single Responsibility** - Varje klass har ett tydligt ansvar
- **Open/Closed** - Öppen för utökning, stängd för modifiering
- **Liskov Substitution** - Interfaces kan ersätta implementationer
- **Interface Segregation** - Små, specifika interfaces
- **Dependency Inversion** - Beroenden genom abstraktioner

### Övriga principer
- **DRY (Don't Repeat Yourself)** - Återanvändbar kod
- **Clean Code** - Läsbar och underhållbar kod
- **Async/Await** - Asynkrona databasanrop
- **Mobile First** - Responsiv design för mobila enheter

##  Funktioner

### Web API
- `GET /api/products` - Hämta alla produkter
- `GET /api/products/favorites` - Hämta favoritprodukter
- `GET /api/products/newest` - Hämta nyaste produkter
- `GET /api/products/{id}` - Hämta specifik produkt

### MVC Webbapplikation
- **Landing Page** - Modern SPA-liknande startsida
- **Produktvisning** - Tre sektioner: Favoriter, Nyheter, Alla produkter
- **Väderwidget** - Realtidsväder i navigationen via extern API
- **Responsiv Design** - Fungerar på alla enheter
- **Animationer** - Smooth transitions och hover-effekter
- **Identity Integration** - Inloggning och registrering

### Console Application
- Visa alla registrerade användare
- Visa komplett produktkatalog i tabellformat
- Lagerstatistik och sammanfattning
- Färgkodad output för bättre läsbarhet

##  Installation & Körning

### Förutsättningar
- .NET 9 SDK
- SQL Server (LocalDB fungerar)
- Visual Studio 2022 eller VS Code

### Steg-för-steg

1. **Klona repository**

2. **Skapa databas (körs automatiskt vid start)**
Migrering körs automatiskt när applikationen startar första gången.

3.  **VIKTIGT**: Välj alltid antingen "MultipleStartupProjects" eller "New Profile" som Startup Project för att applikationen ska köras korrekt.

4. **Starta applikationerna**

### Startup Order

1. API

2. Console

3. Web App

##  Förkonfigurerad Admin-användare

- **Email**: richard.chalk@admin.se
- **Lösenord**: Abc123#

##  Databas

- NatOnNatDb (SQL Server)

### Seedade Produkter
Systemet innehåller 11 förkonfigurerade produkter:
- MacBook Pro 16"
- iPhone 15 Pro Max
- Sony WH-1000XM5
- Samsung Odyssey G9
- Keychron K8 Pro
- NVIDIA RTX 4090
- Samsung 990 PRO 2TB
- ASUS ROG Strix
- Ubiquiti Dream Machine
- iPad Pro 12.9"
- LG C3 OLED 65"

### Connection String
```
"Server=localhost;Database=NatOnNatDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
```


### Input Validering
- Alla användarinput valideras både på klient- och serversidan
- Parameteriserade SQL-frågor via Entity Framework
- XSS-skydd genom Razor-syntax

### Autentisering
- ASP.NET Core Identity för säker användarhantering
- Lösenordskrav: minst 6 tecken, siffror, versaler, gemener och specialtecken
- Säker lösenordshasning


### Optimeringar
- Asynkrona databasanrop
- Lazy loading för bilder
- Responsiv bildstorlek
- Effektiv caching av statiska resurser


### Framtida Funktioner

- Det finns flera knappar och snabblänkar på Landing Page som ej har någon funktion för tillfället. Dessa är reserverade för framtida utveckling.


##  Licens

Detta projekt är skapat för utbildningssyfte.

##  Utvecklare

Utvecklat som del av en utbildningsuppgift inom .NET-utveckling.

---

**NätOnNät** - *Din pålitliga teknikbutik online* 