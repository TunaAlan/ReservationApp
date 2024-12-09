INSERT INTO Restaurants (Category, Name, Address, PhoneNumber, AvgPrice, Capacity, ImageFileName, CreatedAt)
VALUES 
('Seafood', 'Ocean''s Bounty', '12 Shoreline Ave, Seaside', '555-9876', 75, 120, 'oceans_bounty.jpg', GETDATE()),
('Seafood', 'The Fisherman''s Wharf', '10 Ocean Drive, Shoreline City', '555-1111', 85, 150, 'fishermans_wharf.jpg', GETDATE()),

('Fine Dining', 'The Golden Fork', '22 High Street, Uptown', '555-2345', 200, 80, 'golden_fork.jpg', GETDATE()),
('Fine Dining', 'Elegance Palace', '33 Luxury Ave, High Hill', '555-2222', 250, 70, 'elegance_palace.jpg', GETDATE()),

('Fast Food', 'QuickBite', '85 Fast Lane, Speedy City', '555-7654', 15, 60, 'quick_bite.jpg', GETDATE()),
('Fast Food', 'Speedy Bites', '99 Quick Rd, Rushville', '555-3333', 18, 45, 'speedy_bites.jpg', GETDATE()),

('Japanese', 'Sakura Sushi', '4 Blossom Rd, Little Tokyo', '555-3210', 40, 50, 'sakura_sushi.jpg', GETDATE()),
('Japanese', 'Tokyo Delight', '25 Sakura St, New Kyoto', '555-4444', 50, 60, 'tokyo_delight.jpg', GETDATE()),

('Italian', 'Mamma Mia', '15 Olive St, Old Town', '555-9087', 60, 70, 'mamma_mia.jpg', GETDATE()),
('Italian', 'Pasta House', '18 Roman Way, Little Italy', '555-5555', 65, 80, 'pasta_house.jpg', GETDATE()),

('Cafe', 'Brewed Awakening', '9 Coffee Blvd, Downtown', '555-4532', 12, 40, 'brewed_awakening.jpg', GETDATE()),
('Cafe', 'Morning Brew', '44 Bean St, Coffeeville', '555-6666', 15, 35, 'morning_brew.jpg', GETDATE()),

('Steakhouse', 'Grill Master', '34 Beef Rd, Meat District', '555-8723', 90, 90, 'grill_master.jpg', GETDATE()),
('Steakhouse', 'Prime Cut Grill', '52 Steakhouse Rd, Beef City', '555-7777', 100, 85, 'prime_cut_grill.jpg', GETDATE()),

('Bistro', 'Le Petit Bistro', '11 Cozy Corner, Riverside', '555-6789', 45, 30, 'le_petit_bistro.jpg', GETDATE()),
('Bistro', 'Bistro Bella', '3 Cozy Ln, Riverside', '555-8888', 40, 25, 'bistro_bella.jpg', GETDATE());
