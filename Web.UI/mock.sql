-- Kullanıcılar (AspNetUsers)
-- Parolalar semboliktir (Giriş için değil, listelerde görünmesi içindir)
INSERT INTO "AspNetUsers" ("Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount", "FirstName", "LastName", "NationalIdNumber", "IsVerified", "IsBlacklisted", "Active", "Deleted", "CreatedAt")
VALUES 
('usr-001', 'ahmet.yilmaz@test.com', 'AHMET.YILMAZ@TEST.COM', 'ahmet.yilmaz@test.com', 'AHMET.YILMAZ@TEST.COM', 1, 'AQAAAAEAACcQAAAAE...', 'stamp1', 'conc1', '5551234567', 1, 0, 1, 0, 'Ahmet', 'Yılmaz', '11111111111', 1, 0, 1, 0, CURRENT_TIMESTAMP),
('usr-002', 'ayse.kaya@test.com', 'AYSE.KAYA@TEST.COM', 'ayse.kaya@test.com', 'AYSE.KAYA@TEST.COM', 1, 'AQAAAAEAACcQAAAAE...', 'stamp2', 'conc2', '5559876543', 1, 0, 1, 0, 'Ayşe', 'Kaya', '22222222222', 1, 1, 1, 0, CURRENT_TIMESTAMP);

-- Şubeler (Locations)
INSERT INTO "Locations" ("Name", "Address", "City", "District", "PhoneNumber", "Email", "Coordinates", "LocationType", "Active", "Deleted", "CreatedAt")
VALUES 
('Kadıköy Merkez Şube', 'Caferağa Mah. Moda Cad.', 'İstanbul', 'Kadıköy', '02161234567', 'kadikoy@rental420.com', '40.98,29.02', 'Şube', 1, 0, CURRENT_TIMESTAMP),
('Sabiha Gökçen Havalimanı', 'Havalimanı Geliş Terminali', 'İstanbul', 'Pendik', '02169876543', 'saw@rental420.com', '40.90,29.31', 'Havalimanı', 1, 0, CURRENT_TIMESTAMP);

-- Araç Kategorileri (VehicleCategories)
INSERT INTO "VehicleCategories" ("Name", "Description", "DailyPrice", "WeeklyPrice", "MonthlyPrice", "ImageUrl", "MaxPassengers", "BaggageCapacity", "HasAirConditioning", "HasAutomaticTransmission", "HasGPS", "Active", "Deleted", "CreatedAt")
VALUES 
('Ekonomi', 'Şehir içi kullanım için ideal, düşük yakıt tüketimli.', 800.0, 5000.0, 18000.0, '', 5, 2, 1, 1, 0, 1, 0, CURRENT_TIMESTAMP),
('SUV', 'Geniş aileler ve uzun yollar için yüksek donanımlı.', 1500.0, 9500.0, 35000.0, '', 5, 4, 1, 1, 1, 1, 0, CURRENT_TIMESTAMP);

-- Araçlar (Vehicles)
INSERT INTO "Vehicles" ("VehicleCategoryId", "LocationId", "Brand", "Model", "Year", "LicensePlate", "VIN", "Color", "FuelType", "TransmissionType", "Kilometers", "Seats", "FuelTankCapacity", "HasInsurance", "InsuranceExpiryDate", "RequiresInspection", "Notes", "VehicleType", "PricePerDay", "IsAvailable", "Active", "Deleted", "CreatedAt")
VALUES 
(1, 1, 'Renault', 'Clio', 2022, '34ABC123', 'VIN12345678901234', 'Beyaz', 'Benzin', 'Otomatik', 25000, 5, 45.0, 1, '2027-01-01', 0, 'Temiz aile aracı', 'Binek', 850.0, 1, 1, 0, CURRENT_TIMESTAMP),
(2, 2, 'Peugeot', '3008', 2023, '34XYZ789', 'VIN9876543210987', 'Siyah', 'Dizel', 'Otomatik', 12000, 5, 55.0, 1, '2027-05-01', 0, 'Full paket', 'SUV', 1600.0, 1, 1, 0, CURRENT_TIMESTAMP);

-- Kiralamalar (Rentals)
INSERT INTO "Rentals" ("VehicleId", "MemberId", "PickUpLocationId", "DropOffLocationId", "StartDate", "EndDate", "TotalAmount", "RentalStatus", "PaymentStatus", "PickUpNotes", "Active", "Deleted", "CreatedAt")
VALUES 
(1, 'usr-001', 1, 1, CURRENT_TIMESTAMP, date(CURRENT_TIMESTAMP, '+3 days'), 2550.0, 'Aktif', 'Ödendi', 'Araç temiz teslim edildi', 1, 0, CURRENT_TIMESTAMP),
(2, 'usr-002', 2, 1, date(CURRENT_TIMESTAMP, '-5 days'), date(CURRENT_TIMESTAMP, '-2 days'), 4800.0, 'Tamamlandı', 'Ödendi', '', 1, 0, CURRENT_TIMESTAMP);

-- Faturalar (Invoices)
INSERT INTO "Invoices" ("RentalId", "MemberId", "InvoiceNumber", "IssueDate", "DueDate", "TotalAmount", "TaxAmount", "InvoiceStatus", "Notes", "Active", "Deleted", "CreatedAt")
VALUES 
(1, 'usr-001', 'INV-2026-0001', CURRENT_TIMESTAMP, date(CURRENT_TIMESTAMP, '+7 days'), 2550.0, 459.0, 'Ödendi', '', 1, 0, CURRENT_TIMESTAMP),
(2, 'usr-002', 'INV-2026-0002', date(CURRENT_TIMESTAMP, '-5 days'), date(CURRENT_TIMESTAMP, '+2 days'), 4800.0, 864.0, 'Ödendi', '', 1, 0, CURRENT_TIMESTAMP);

-- Ödemeler (Payments)
INSERT INTO "Payments" ("InvoiceId", "RentalId", "MemberId", "Amount", "PaymentDate", "PaymentMethod", "PaymentStatus", "TransactionId", "Active", "Deleted", "CreatedAt")
VALUES 
(1, 1, 'usr-001', 2550.0, CURRENT_TIMESTAMP, 'Kredi Kartı', 'Başarılı', 'TRX9988776655', 1, 0, CURRENT_TIMESTAMP),
(2, 2, 'usr-002', 4800.0, date(CURRENT_TIMESTAMP, '-5 days'), 'Nakit', 'Başarılı', 'TRX1122334455', 1, 0, CURRENT_TIMESTAMP);

-- Değerlendirmeler (Reviews)
INSERT INTO "Reviews" ("RentalId", "VehicleId", "MemberId", "Rating", "Comment", "IsApproved", "Active", "Deleted", "CreatedAt")
VALUES 
(2, 2, 'usr-002', 5, 'Çok güzel bir kiralama deneyimiydi, teşekkürler.', 1, 1, 0, CURRENT_TIMESTAMP);

-- Masraf & Cezalar (Penalties)
INSERT INTO "Penalties" ("RentalId", "MemberId", "VehicleId", "PenaltyType", "Description", "PenaltyAmount", "PenaltyStatus", "Notes", "Active", "Deleted", "CreatedAt")
VALUES 
(2, 'usr-002', 2, 'Geç İade', 'Araç 2 saat geç teslim edildi', 500.0, 'Beklemede', 'Müşteriye bilgi verildi', 1, 0, CURRENT_TIMESTAMP);

-- Kampanyalar (Campaigns)
INSERT INTO "Campaigns" ("Name", "Description", "DiscountType", "DiscountValue", "StartDate", "EndDate", "IsActive", "CampaignCode", "Active", "Deleted", "CreatedAt")
VALUES 
('Yaz Fırsatı', 'Yaz aylarında tüm araçlarda %15 indirim', 'Yüzde', 15.0, CURRENT_TIMESTAMP, date(CURRENT_TIMESTAMP, '+30 days'), 1, 'YAZ15', 1, 0, CURRENT_TIMESTAMP);
