USE [Ljus_och_Miljo_DB]

INSERT INTO [dbo].[TagTyp] ([DisplayName]) VALUES 
('Lampa'),
('Stol'),
('Gräs'),
('Bord'),
('Solpanel'),
('Kniv'),
('Elbil'),
('Laddare'),
('Batteri'),
('Däck'),
('Trä'),
('Golv'),
('Färg'),
('Jord'),
('Växter'),
('werner'),
('Röd');

INSERT INTO [dbo].[Product]
           ([DisplayName]
           ,[ProductDesc]
           ,[ProductPrice]
           ,[Imagealink])
     VALUES


('Global Light'	,	'Miljövänlig lampa.' ,	149,''),
('Green Green Grass'	,	'Snabbväxande gräs.',	99,''),
('Beacon of Light'	,	'Stark miljövänlig lampa.',	299,''),
('Reborn Chair'	,	'En stol skapad utav återvunnet material.',	399,''),
('Car Charger'	,	'Elbil laddningsstation.',	1999,''),
('Knife Set'	,	'Knivar gjorda av miljövänligt material.'	,89,''),
('Green Light'	,	'Grön lampa med starkt ljus'	,49	,''),
('Red Light'	,	'Röd lampa'	,49,''),
('Palm Tree'	,	'Palm i kruka'	,599,''),
('Fern' ,	'Ormbunke',	19,''),
('Blue Berry Bush'	,	'Blåbärsbuske'	,99	,''),
('Olive Tree'	,	'Olivträd',	899	,''),
('Kim Stol'	,	'Inte Kims Stol'	,100	,''),
('Stolen Roger'	,	'En grön stol'	,200	,''),
('Apan Donald'	,	'Ett mysigt gosedjur'	,99	,''),
('Frisbee'	,	'En rund grej man kan kasta till varandra, väldigt trevligt att ha med på picknick'	,155	,''),
('Ljus ljus'	,	'ett vackert ljus'	,2000	,''),
('Advent ljusstake'	,	'Vackert advent jul ljus ljus',	5999,''),
('Bordsljus'	,	'Vattenljusen'	,7999	,''),
('Purple Light'	,	'En utelampa'	,433	,''),
('lampa'	,	'blue lamp'	,400	,''),
('Newton lampa'	,	'En ute lampa'	,55	,''),
('Stol' ,	'En praktisk sak man kan sitta på'	,234,'');
