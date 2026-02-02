# 🚗 Softver za upravljanje voznim parkom

MVVM WPF aplikacija uz upotrebu Entity Framework Core i UML modelovanja.  
Projekat razvijen u okviru kursa **Dizajn i razvoj softvera**.

---

## 📌 Sadržaj
- [Opis projekta](#opis-projekta)
- [Tehnologije i alati](#tehnologije-i-alati)
- [Funkcionalnosti](#funkcionalnosti)
- [Arhitektura](#arhitektura)
- [Instalacija i pokretanje](#instalacija-i-pokretanje)
- [Testiranje](#testiranje)
- [Dokumentacija](#dokumentacija)

---

## 📖 Opis projekta
Aplikacija omogućava administratorima, menadžerima i vozačima da upravljaju voznim parkom.  
Podaci se čuvaju u SQLite bazi, a aplikacija koristi MVVM arhitekturu i Entity Framework Core.

---

## 🛠️ Tehnologije i alati
- **UI:** WPF (.NET 6+/8), XAML  
- **Arhitektura:** MVVM (Model-View-ViewModel)  
- **ORM:** Entity Framework Core  
- **Baza podataka:** SQLite  
- **Verzionisanje:** Git, GitHub  
- **Testiranje:** xUnit + EF Core InMemory provider  
- **Serijalizacija:** JSON (System.Text.Json), XML (XmlSerializer)  
- **Dokumentacija:** UML dijagrami (draw.io, PlantUML)  

---

## ⚙️ Funkcionalnosti
- Dodavanje novog vozila  
- Izmena i brisanje vozila  
- Autentifikacija (login forma)  
- Pregled i filtriranje vozila  
- Pregled sopstvenih vozila (za vozače)  
- Generisanje PDF izveštaja o voznom parku  
- Serijalizacija podataka u JSON/XML  

---

## 🏗️ Arhitektura
- **Model:** Entiteti `Vozilo`, `Servis`, `Osoba` (apstraktna klasa), izvedene klase `Administrator`, `Menadžer`, `Vozač`.  
- **ViewModel:** Logika aplikacije, ICommand implementacija, validacija.  
- **View:** XAML forme (Login, Vozila, Servisi, Izveštaji).  
- **Dizajn šabloni:**  
  - Factory
  - Observer (INotifyPropertyChanged)  

---

## 🚀 Instalacija i pokretanje
1. Kloniraj repozitorijum:
   ```bash
   git clone https://github.com/username/FleetManagement.git
2. Uđi u folder projekta:
   cd FleetManagement
3. Pokreni aplikaciju:
   dotnet run --project FleetManagement

🧪 Testiranje
Testovi se nalaze u projektu FleetManagement.Tests.
Pokretanje testova:
  dotnet test
