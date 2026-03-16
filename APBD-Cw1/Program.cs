using APBD_Cw1.Configuration;
using APBD_Cw1.Domain;
using APBD_Cw1.Services;

var idGenerator = new IdGenerator();
var rentalService = new RentalService(idGenerator, BusinessRules.Default);
var reportService = new ReportService();

Console.WriteLine("=== UCZELNIANA WYPOŻYCZALNIA SPRZĘTU ===");
Console.WriteLine();

// 1. Dodanie kilku egzemplarzy sprzętu różnych typów
var laptop1 = new Laptop(idGenerator.NextEquipmentId(), "Dell Latitude 7440", "Intel i7", 16);
var laptop2 = new Laptop(idGenerator.NextEquipmentId(), "Lenovo ThinkPad T14", "AMD Ryzen 7", 32);
var projector1 = new Projector(idGenerator.NextEquipmentId(), "Epson EB-FH52", 4000, true);
var camera1 = new Camera(idGenerator.NextEquipmentId(), "Canon EOS R10", 24, true);

rentalService.AddEquipment(laptop1);
rentalService.AddEquipment(laptop2);
rentalService.AddEquipment(projector1);
rentalService.AddEquipment(camera1);

// 2. Dodanie kilku użytkowników różnych typów
var student1 = new Student(idGenerator.NextUserId(), "Jan", "Kowalski");
var student2 = new Student(idGenerator.NextUserId(), "Anna", "Nowak");
var employee1 = new Employee(idGenerator.NextUserId(), "Piotr", "Wiśniewski");

rentalService.AddUser(student1);
rentalService.AddUser(student2);
rentalService.AddUser(employee1);

Console.WriteLine("Sprzęt w systemie:");
foreach (var equipment in rentalService.GetAllEquipment())
{
    Console.WriteLine(equipment);
}

Console.WriteLine();
Console.WriteLine("Użytkownicy w systemie:");
foreach (var user in rentalService.GetAllUsers())
{
    Console.WriteLine(user);
}

// 3. Poprawne wypożyczenie sprzętu
Console.WriteLine();
Console.WriteLine("=== Poprawne wypożyczenia ===");

var rentResult1 = rentalService.RentEquipment(student1.Id, laptop1.Id, DateTime.Today, 7);
Console.WriteLine(rentResult1.Message);

var rentResult2 = rentalService.RentEquipment(employee1.Id, projector1.Id, DateTime.Today, 3);
Console.WriteLine(rentResult2.Message);

// 4. Próba niepoprawnej operacji - wypożyczenie sprzętu niedostępnego
Console.WriteLine();
Console.WriteLine("=== Próba błędnej operacji ===");

var invalidRent = rentalService.RentEquipment(student2.Id, laptop1.Id, DateTime.Today, 5);
Console.WriteLine(invalidRent.Message);

// Dodatkowo: przekroczenie limitu dla studenta
var extraCamera = new Camera(idGenerator.NextEquipmentId(), "Sony Alpha A6400", 24, false);
var extraProjector = new Projector(idGenerator.NextEquipmentId(), "BenQ MX560", 3600, false);

rentalService.AddEquipment(extraCamera);
rentalService.AddEquipment(extraProjector);

Console.WriteLine(rentalService.RentEquipment(student1.Id, camera1.Id, DateTime.Today, 4).Message);
Console.WriteLine(rentalService.RentEquipment(student1.Id, extraCamera.Id, DateTime.Today, 2).Message);

// 5. Zwrot sprzętu w terminie
Console.WriteLine();
Console.WriteLine("=== Zwrot w terminie ===");

var activeRentalForProjector = rentalService
    .GetActiveRentalsForUser(employee1.Id)
    .First(r => r.EquipmentId == projector1.Id);

var returnOnTime = rentalService.ReturnEquipment(activeRentalForProjector.Id, DateTime.Today.AddDays(2));
Console.WriteLine(returnOnTime.Message);

// 6. Zwrot opóźniony skutkujący naliczeniem kary
Console.WriteLine();
Console.WriteLine("=== Zwrot opóźniony ===");

var activeRentalForLaptop = rentalService
    .GetActiveRentalsForUser(student1.Id)
    .First(r => r.EquipmentId == laptop1.Id);

var lateReturn = rentalService.ReturnEquipment(activeRentalForLaptop.Id, DateTime.Today.AddDays(10));
Console.WriteLine(lateReturn.Message);

// 7. Oznaczenie sprzętu jako niedostępnego
Console.WriteLine();
Console.WriteLine("=== Oznaczenie sprzętu jako niedostępnego ===");
Console.WriteLine(rentalService.MarkEquipmentAsUnavailable(laptop2.Id, "Serwis techniczny").Message);

// 8. Wyświetlenie aktywnych wypożyczeń danego użytkownika
Console.WriteLine();
Console.WriteLine($"=== Aktywne wypożyczenia użytkownika: {student1.FullName} ===");
foreach (var rental in rentalService.GetActiveRentalsForUser(student1.Id))
{
    Console.WriteLine(rental);
}

// 9. Wyświetlenie listy przeterminowanych wypożyczeń
Console.WriteLine();
Console.WriteLine($"=== Przeterminowane wypożyczenia na dzień {DateTime.Today.AddDays(10):dd.MM.yyyy} ===");

foreach (var rental in rentalService.GetOverdueRentals(DateTime.Today.AddDays(10)))
{
    Console.WriteLine(rental);
}

// 10. Raport końcowy
Console.WriteLine();
Console.WriteLine("=== RAPORT KOŃCOWY ===");
Console.WriteLine(reportService.BuildSummary(rentalService, DateTime.Today.AddDays(10)));