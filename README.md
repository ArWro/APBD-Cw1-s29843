# Uczelniana wypożyczalnia sprzętu

## Opis projektu

To prosta aplikacja konsolowa w C#, która obsługuje uczelnianą wypożyczalnię sprzętu.  
Pozwala dodawać użytkowników i sprzęt, wypożyczać i zwracać urządzenia, sprawdzać dostępność, oznaczać sprzęt jako niedostępny, wyświetlać przeterminowane wypożyczenia oraz generować krótki raport.

---

## Podział projektu

### `Domain`
Tutaj znajdują się klasy opisujące model domeny, czyli najważniejsze obiekty w systemie:

- `Equipment` oraz jego klasy pochodne: `Laptop`, `Projector`, `Camera`
- `User` oraz jego klasy pochodne: `Student`, `Employee`
- `Rental`
- enumy `EquipmentStatus` i `UserType`

### `Services`
Tutaj jest logika działania aplikacji:

- `RentalService` obsługuje wypożyczenia, zwroty i sprawdzanie reguł biznesowych,
- `ReportService` tworzy raport podsumowujący,
- `IdGenerator` generuje identyfikatory,
- `Result` zwraca informację, czy operacja się udała.

### `Configuration`
- `BusinessRules` przechowuje limity wypożyczeń i sposób naliczania kar.

### `Program.cs`
Służy tylko do uruchomienia przykładowego scenariusza i wyświetlenia wyników w konsoli.

---

## Dlaczego taki podział

Chciałem oddzielić dane i obiekty domenowe od logiki biznesowej oraz od obsługi konsoli. Dzięki temu kod jest prostszy do czytania i łatwiej zobaczyć, która klasa za co odpowiada.

---

## Gdzie widać kohezję i odpowiedzialności klas

Starałem się, żeby każda klasa miała jedno główne zadanie:

- `Equipment` odpowiada za wspólne cechy sprzętu i jego status,
- `Rental` przechowuje dane o pojedynczym wypożyczeniu,
- `RentalService` wykonuje operacje biznesowe,
- `ReportService` zajmuje się tylko raportem,
- `IdGenerator` tylko generuje identyfikatory,
- `BusinessRules` trzyma reguły, które mogą się zmieniać.

Dzięki temu klasy nie są przypadkowym zbiorem metod.

---

## Gdzie widać próbę ograniczenia coupling

Starałem się nie mieszać wszystkiego w jednym miejscu:

- `Program.cs` nie zawiera logiki biznesowej,
- reguły wypożyczeń i kar nie są rozsiane po całym projekcie, tylko znajdują się w `BusinessRules`,
- raportowanie jest oddzielone od obsługi wypożyczeń,
- klasy domenowe nie zajmują się wypisywaniem menu ani obsługą logiki aplikacji.

To zmniejsza zależności między klasami i ułatwia późniejsze zmiany.

---

## Dziedziczenie

Dziedziczenie zostało użyte tylko tam, gdzie miało sens w modelu domeny:

- różne typy sprzętu dziedziczą po `Equipment`,
- różne typy użytkowników dziedziczą po `User`.

Wybrałem takie podejście, bo te obiekty mają wspólne cechy, ale też własne pola specyficzne.

---

## Podsumowanie

Projekt jest prosty, ale celowo został podzielony na kilka obszarów, żeby było widać:

- sensowny podział odpowiedzialności,
- próbę zachowania kohezji,
- ograniczenie sprzężenia między klasami,
- oddzielenie modelu, logiki biznesowej i warstwy uruchomieniowej.
