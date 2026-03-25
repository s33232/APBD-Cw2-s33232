# Wypożyczalnia sprzętu uczelnianego (APBD - Ćwiczenia 2)

Aplikacja konsolowa do zarządzania wypożyczeniami sprzętu, napisana z naciskiem na architekturę i dobre praktyki programowania obiektowego.

## Instrukcja uruchomienia
Projekt można uruchomić standardowo z poziomu IDE (JetBrains Rider / Visual Studio) za pomocą przycisku Run (F5) lub z poziomu terminala, wpisując `dotnet run` w katalogu projektu. W pliku `Program.cs` znajduje się gotowy scenariusz demonstracyjny, który sam wykonuje operacje dodawania, wypożyczania i zwrotów.

## Uzasadnienie decyzji projektowych

Zgodnie z poleceniem, starałem się uniknąć wrzucenia całego kodu do jednego "wielkiego" pliku i skupiłem się na sensownym podziale odpowiedzialności.

### 1. Podział na warstwy i pliki
Projekt podzieliłem na dwa główne katalogi:
* **Domain (Modele):** Tutaj trzymam same "głupie" obiekty reprezentujące dane (`Equipment`, `User`, `Rental`). Nie mają one pojęcia o tym, jak działa system kar czy walidacja.
* **Services (Logika):** Tutaj znajduje się mózg operacji, czyli `RentalManager`.
  Dzięki takiemu podziałowi `Program.cs` zajmuje się tylko wyświetlaniem tekstu i "spinaniem" klocków (pełni rolę prostego UI), a nie ukrywa w sobie reguł biznesowych.

### 2. Dziedziczenie vs Kompozycja
Użyłem klas abstrakcyjnych dla bazowego sprzętu (`Equipment`) i użytkownika (`User`). Uznałem, że to lepsze wyjście, bo każdy sprzęt ma zawsze Id i nazwę, a każdy użytkownik ma imię i nazwisko.
Zamiast pisać w menedżerze drabinki `if (user.Type == "Student")`, zadeklarowałem abstrakcyjną właściwość `MaxActiveRentals`. Każda klasa pochodna (`Student`, `Employee`) sama definiuje swój limit. To mocno czyści kod logiki.

### 3. Kohezja i Coupling (Zarządzanie odpowiedzialnością)
* **Wysoka kohezja w RentalManager:** Ten serwis odpowiada wyłącznie za procesowanie wypożyczeń (sprawdzanie dostępności sprzętu i limitów użytkownika).
* **Niski coupling przy obliczaniu kar:** Nie chciałem zaszywać matematyki liczenia kar bezpośrednio w `RentalManagerze`. Zamiast tego stworzyłem interfejs `IPenaltyCalculator` i wstrzyknąłem go przez konstruktor. Jeśli kiedyś zmienią się reguły (np. kary nie będą naliczane w weekendy), napiszę nową klasę implementującą interfejs, a kod menedżera pozostanie nietknięty (zasada Open/Closed).

### 4. Obsługa błędów
Jeśli użytkownik próbuje wypożyczyć niedostępny sprzęt albo przekracza swój limit, system jawnie rzuca wyjątek `InvalidOperationException`. Uznałem, że to bardziej czytelne niż zwracanie z metod wartości `null` albo `false`, bo od razu wiadomo, która reguła biznesowa została złamana.