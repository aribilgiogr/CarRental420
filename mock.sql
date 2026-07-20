-- Önceki mock verilerini temizleyelim (Çakışma olmaması için)
DELETE FROM "Campaigns" WHERE "CouponCode" = 'YAZ15';
DELETE FROM "Penalties" WHERE "MemberId" IN ('usr-001', 'usr-002');
DELETE FROM "Reviews" WHERE "MemberId" IN ('usr-001', 'usr-002');
DELETE FROM "Payments" WHERE "RentalId" IN (SELECT "Id" FROM "Rentals" WHERE "MemberId" IN ('usr-001', 'usr-002'));
DELETE FROM "Invoices" WHERE "MemberId" IN ('usr-001', 'usr-002');
DELETE FROM "Rentals" WHERE "MemberId" IN ('usr-001', 'usr-002');
DELETE FROM "Vehicles" WHERE "LicensePlate" IN ('34ABC123', '34XYZ789');
DELETE FROM "VehicleCategories" WHERE "Name" IN ('Ekonomi', 'SUV');
DELETE FROM "Locations" WHERE "Name" IN ('Kadıköy Merkez Şube', 'Sabiha Gökçen Havalimanı');
DELETE FROM "AspNetUsers" WHERE "Id" IN ('usr-001', 'usr-002');

-- Kullanıcılar (AspNetUsers)
INSERT INTO "AspNetUsers" ("Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount", "FirstName", "LastName", "NationalIdNumber", "IsVerified", "IsBlacklisted", "Deleted", "CreatedAt")
VALUES 
('usr-001', 'ahmet.yilmaz@test.com', 'AHMET.YILMAZ@TEST.COM', 'ahmet.yilmaz@test.com', 'AHMET.YILMAZ@TEST.COM', 1, 'AQAAAAEAACcQAAAAE...', 'stamp1', 'conc1', '5551234567', 1, 0, 1, 0, 'Ahmet', 'Yılmaz', '11111111111', 1, 0, 0, CURRENT_TIMESTAMP),
('usr-002', 'ayse.kaya@test.com', 'AYSE.KAYA@TEST.COM', 'ayse.kaya@test.com', 'AYSE.KAYA@TEST.COM', 1, 'AQAAAAEAACcQAAAAE...', 'stamp2', 'conc2', '5559876543', 1, 0, 1, 0, 'Ayşe', 'Kaya', '22222222222', 1, 1, 0, CURRENT_TIMESTAMP);

-- Şubeler (Locations)
INSERT INTO "Locations" ("Id", "Name", "City", "District", "Street", "PostalCode", "PhoneNumber", "Email", "Manager", "Latitude", "Longitude", "OpeningTime", "ClosingTime", "IsOpen24Hours", "Active", "Deleted", "CreatedAt")
VALUES 
(901, 'Kadıköy Merkez Şube', 'İstanbul', 'Kadıköy', 'Caferağa Mah. Moda Cad.', '34710', '02161234567', 'kadikoy@rental420.com', 'Müdür Ali', 40.98, 29.02, '09:00', '18:00', 0, 1, 0, CURRENT_TIMESTAMP),
(902, 'Sabiha Gökçen Havalimanı', 'İstanbul', 'Pendik', 'Havalimanı Geliş Terminali', '34906', '02169876543', 'saw@rental420.com', 'Müdür Ayşe', 40.90, 29.31, '00:00', '23:59', 1, 1, 0, CURRENT_TIMESTAMP);

-- Araç Kategorileri (VehicleCategories)
INSERT INTO "VehicleCategories" ("Id", "Name", "Description", "DailyPrice", "WeeklyPrice", "MonthlyPrice", "ImageUrl", "MaxPassengers", "BaggageCapacity", "HasAirConditioning", "HasAutomaticTransmission", "HasGPS", "Active", "Deleted", "CreatedAt")
VALUES 
(901, 'Ekonomi', 'Şehir içi kullanım için ideal, düşük yakıt tüketimli.', 800.0, 5000.0, 18000.0, '', 5, 2, 1, 1, 0, 1, 0, CURRENT_TIMESTAMP),
(902, 'SUV', 'Geniş aileler ve uzun yollar için yüksek donanımlı.', 1500.0, 9500.0, 35000.0, '', 5, 4, 1, 1, 1, 1, 0, CURRENT_TIMESTAMP);

-- Araçlar (Vehicles)
INSERT INTO "Vehicles" ("Id", "VehicleCategoryId", "LocationId", "Brand", "Model", "Year", "LicensePlate", "VIN", "Color", "FuelType", "TransmissionType", "Kilometers", "Seats", "FuelTankCapacity", "HasInsurance", "InsuranceExpiryDate", "RequiresInspection", "Notes", "VehicleType", "PricePerDay", "IsAvailable", "Active", "Deleted", "CreatedAt")
VALUES 
(901, 901, 901, 'Renault', 'Clio', 2022, '34ABC123', 'VIN12345678901234', 'Beyaz', 'Benzin', 'Otomatik', 25000, 5, 45.0, 1, '2027-01-01', 0, 'Temiz aile aracı', 'Binek', 850.0, 1, 1, 0, CURRENT_TIMESTAMP),
(902, 902, 902, 'Peugeot', '3008', 2023, '34XYZ789', 'VIN9876543210987', 'Siyah', 'Dizel', 'Otomatik', 12000, 5, 55.0, 1, '2027-05-01', 0, 'Full paket', 'SUV', 1600.0, 1, 1, 0, CURRENT_TIMESTAMP);

-- Kiralamalar (Rentals)
INSERT INTO "Rentals" ("Id", "MemberId", "VehicleId", "StartLocationId", "EndLocationId", "RentalStartDate", "RentalEndDate", "StartOdometer", "TotalPrice", "DiscountAmount", "FinalPrice", "RentalStatus", "IsReturned", "HasDamage", "RentalDays", "Active", "Deleted", "CreatedAt")
VALUES 
(901, 'usr-001', 901, 901, 901, CURRENT_TIMESTAMP, date(CURRENT_TIMESTAMP, '+3 days'), 25000, 2550.0, 0, 2550.0, 'Aktif', 0, 0, 3, 1, 0, CURRENT_TIMESTAMP),
(902, 'usr-002', 902, 902, 901, date(CURRENT_TIMESTAMP, '-5 days'), date(CURRENT_TIMESTAMP, '-2 days'), 12000, 4800.0, 0, 4800.0, 'Tamamlandı', 1, 0, 3, 1, 0, CURRENT_TIMESTAMP);

-- Faturalar (Invoices)
INSERT INTO "Invoices" ("InvoiceNumber", "RentalId", "MemberId", "InvoiceDate", "SubTotal", "TaxAmount", "DiscountAmount", "TotalAmount", "InvoiceStatus", "DueDate", "Active", "Deleted", "CreatedAt")
VALUES 
('INV-2026-0001', 901, 'usr-001', CURRENT_TIMESTAMP, 2091.0, 459.0, 0, 2550.0, 'Ödendi', date(CURRENT_TIMESTAMP, '+7 days'), 1, 0, CURRENT_TIMESTAMP),
('INV-2026-0002', 902, 'usr-002', date(CURRENT_TIMESTAMP, '-5 days'), 3936.0, 864.0, 0, 4800.0, 'Ödendi', date(CURRENT_TIMESTAMP, '+2 days'), 1, 0, CURRENT_TIMESTAMP);

-- Ödemeler (Payments)
INSERT INTO "Payments" ("RentalId", "Amount", "PaymentMethod", "PaymentStatus", "PaymentDate", "TransactionNumber", "Active", "Deleted", "CreatedAt")
VALUES 
(901, 2550.0, 'Kredi Kartı', 'Başarılı', CURRENT_TIMESTAMP, 'TRX9988776655', 1, 0, CURRENT_TIMESTAMP),
(902, 4800.0, 'Nakit', 'Başarılı', date(CURRENT_TIMESTAMP, '-5 days'), 'TRX1122334455', 1, 0, CURRENT_TIMESTAMP);

-- Değerlendirmeler (Reviews)
INSERT INTO "Reviews" ("RentalId", "MemberId", "Rating", "Title", "Comment", "IsVerified", "IsPublished", "ReviewDate", "Active", "Deleted", "CreatedAt")
VALUES 
(902, 'usr-002', 5, 'Harika Deneyim', 'Çok güzel bir kiralama deneyimiydi, teşekkürler.', 1, 1, CURRENT_TIMESTAMP, 1, 0, CURRENT_TIMESTAMP);

-- Masraf & Cezalar (Penalties)
INSERT INTO "Penalties" ("RentalId", "MemberId", "VehicleId", "PenaltyType", "Description", "PenaltyAmount", "PenaltyStatus", "Notes", "Active", "Deleted", "CreatedAt")
VALUES 
(902, 'usr-002', 902, 'Geç İade', 'Araç 2 saat geç teslim edildi', 500.0, 'Beklemede', 'Müşteriye bilgi verildi', 1, 0, CURRENT_TIMESTAMP);

-- Kampanyalar (Campaigns)
INSERT INTO "Campaigns" ("Name", "Description", "CampaignType", "CampaignScope", "StartDate", "EndDate", "DiscountPercentage", "IsActive", "CouponCode", "Active", "Deleted", "CreatedAt")
VALUES 
('Yaz Fırsatı', 'Yaz aylarında tüm araçlarda %15 indirim', 1, 1, CURRENT_TIMESTAMP, date(CURRENT_TIMESTAMP, '+30 days'), 15.0, 1, 'YAZ15', 1, 0, CURRENT_TIMESTAMP);
