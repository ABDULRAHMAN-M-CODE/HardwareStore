USE [HardwareDB]
GO

/****** Object:  StoredProcedure [dbo].[uspDeleteAllDataFromDatabase]    Script Date: 10/22/2026 9:41:22 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[uspDeleteAllDataFromDatabase]

AS
BEGIN

    SET NOCOUNT ON;
--Deleting content of junction tables
DELETE FROM BrandsSuppliers
DELETE FROM ProductsBrands
DELETE FROM ProductsBins
DELETE FROM ProductsManufacturers
DELETE FROM ProductsSuppliers
DELETE FROM ProductsCategories
DELETE FROM ProductsCountries
DELETE FROM ProductsUnits
--Deleting content of non-junction child tables 
DELETE FROM SubCategories
--Deleting content of parent tables
-- Deleting parent tables (tables that does not depend on any other table)
DELETE FROM Bins
DELETE FROM Brands
DELETE FROM Categories
DELETE FROM Countries
DELETE FROM Manufacturers
DELETE FROM Products
DELETE FROM Units
DELETE FROM Suppliers
END;
GO

