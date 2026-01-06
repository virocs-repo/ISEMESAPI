-- =============================================
-- Stored Procedure: PRD_Device_P_GetForEditView
-- Description: Retrieves complete device information for edit/view operations
-- Parameters: @DeviceId (required) - The ID of the device to retrieve
-- Returns: Multiple result sets with device details, alias names, and label information
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[PRD_Device_P_GetForEditView]
    @DeviceId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Validate input parameter
    IF @DeviceId IS NULL OR @DeviceId <= 0
    BEGIN
        RAISERROR('DeviceId must be provided and greater than 0', 16, 1);
        RETURN;
    END

    -- Result Set 1: Main Device Information
    SELECT 
        d.DeviceId,
        d.DeviceName,
        d.DeviceFamilyID,
        df.DeviceFamily AS DeviceFamilyName,
        df.CustomerDeviceFamily,
        d.CustomerID,
        c.CustomerName,
        d.isActive AS Active,
        d.TestDevice,
        d.ReliabilityDevice,
        d.SKU,
        d.PartTypeId,
        pt.PartTypeName,
        d.DeviceTypeId,
        dt.DeviceTypeName AS DeviceTypeName,
        d.isLabelMapped AS IsLabelMapped,
        d.IsDeviceBasedTray,
        d.COOId AS CountryOfOriginId,
        coo.CountryName AS CountryOfOriginName,
        d.UnitCost,
        d.MaterialDescriptionId,
        md.MaterialDescriptionName,
        d.USHTSCodeId,
        ushts.USHTSCodeName,
        d.ECCNId,
        eccn.ECCNName,
        d.LicenseExceptionId,
        le.LicenseExceptionName,
        d.RestrictedCountriesToShipId,
        d.ScheduleB,
        d.MSLId,
        msl.MSLName,
        d.PeakPackageBodyTemperatureId,
        ppbt.TemperatureName AS PeakPackageBodyTemperatureName,
        d.ShelfLifeMonthId,
        slm.MonthName AS ShelfLifeMonthName,
        d.FloorLifeId,
        fl.FloorLifeName,
        d.PBFreeId,
        pbf.PBFreeName,
        d.PBFreeStickerId,
        pbfs.PBFreeStickerName,
        d.ROHSId,
        rohs.ROHSName,
        d.TrayTubeStrappingId,
        tts.StrappingName AS TrayTubeStrappingName,
        d.TrayStackingId,
        ts.StackingName AS TrayStackingName,
        d.LockId,
        d.LastModifiedOn,
        d.CreatedOn,
        d.ModifiedOn,
        d.CreatedBy,
        d.ModifiedBy,
        -- Check if device can be edited (not locked or no active transactions)
        CASE 
            WHEN d.LockId IS NOT NULL AND d.LockId > 0 THEN 0
            WHEN EXISTS (
                SELECT 1 
                FROM PRD_Inventory i 
                WHERE i.DeviceId = d.DeviceId 
                AND i.Active = 1
            ) THEN 0
            ELSE 1
        END AS CanEdit,
        -- Check if lot type can be edited
        CASE 
            WHEN EXISTS (
                SELECT 1 
                FROM PRD_Inventory i 
                WHERE i.DeviceId = d.DeviceId 
                AND i.Active = 1
            ) THEN 0
            ELSE 1
        END AS CanEditLotType,
        -- Check if labels can be edited (similar logic)
        CASE 
            WHEN EXISTS (
                SELECT 1 
                FROM PRD_Inventory i 
                WHERE i.DeviceId = d.DeviceId 
                AND i.Active = 1
            ) THEN 0
            ELSE 1
        END AS CanEditLabel1,
        CASE 
            WHEN EXISTS (
                SELECT 1 
                FROM PRD_Inventory i 
                WHERE i.DeviceId = d.DeviceId 
                AND i.Active = 1
            ) THEN 0
            ELSE 1
        END AS CanEditLabel2,
        CASE 
            WHEN EXISTS (
                SELECT 1 
                FROM PRD_Inventory i 
                WHERE i.DeviceId = d.DeviceId 
                AND i.Active = 1
            ) THEN 0
            ELSE 1
        END AS CanEditLabel3,
        CASE 
            WHEN EXISTS (
                SELECT 1 
                FROM PRD_Inventory i 
                WHERE i.DeviceId = d.DeviceId 
                AND i.Active = 1
            ) THEN 0
            ELSE 1
        END AS CanEditLabel4,
        CASE 
            WHEN EXISTS (
                SELECT 1 
                FROM PRD_Inventory i 
                WHERE i.DeviceId = d.DeviceId 
                AND i.Active = 1
            ) THEN 0
            ELSE 1
        END AS CanEditLabel5
    FROM PRD_Device d
    LEFT JOIN PRD_DeviceFamily df ON d.DeviceFamilyID = df.DeviceFamilyId
    LEFT JOIN Customer c ON d.CustomerID = c.CustomerId
    LEFT JOIN PartType pt ON d.PartTypeId = pt.PartTypeId
    LEFT JOIN DeviceType dt ON d.DeviceTypeId = dt.DeviceTypeId
    LEFT JOIN Country coo ON d.COOId = coo.CountryId
    LEFT JOIN MaterialDescription md ON d.MaterialDescriptionId = md.MaterialDescriptionId
    LEFT JOIN USHTSCode ushts ON d.USHTSCodeId = ushts.USHTSCodeId
    LEFT JOIN ECCN eccn ON d.ECCNId = eccn.ECCNId
    LEFT JOIN LicenseException le ON d.LicenseExceptionId = le.LicenseExceptionId
    LEFT JOIN MSL msl ON d.MSLId = msl.MSLId
    LEFT JOIN PeakPackageBodyTemperature ppbt ON d.PeakPackageBodyTemperatureId = ppbt.TemperatureId
    LEFT JOIN ShelfLifeMonth slm ON d.ShelfLifeMonthId = slm.MonthId
    LEFT JOIN FloorLife fl ON d.FloorLifeId = fl.FloorLifeId
    LEFT JOIN PBFree pbf ON d.PBFreeId = pbf.PBFreeId
    LEFT JOIN PBFreeSticker pbfs ON d.PBFreeStickerId = pbfs.PBFreeStickerId
    LEFT JOIN ROHS rohs ON d.ROHSId = rohs.ROHSId
    LEFT JOIN TrayTubeStrapping tts ON d.TrayTubeStrappingId = tts.StrappingId
    LEFT JOIN TrayStacking ts ON d.TrayStackingId = ts.StackingId
    WHERE d.DeviceId = @DeviceId;

    -- Result Set 2: Device Alias Names
    SELECT 
        dan.AliasId,
        dan.DeviceId,
        dan.AliasName,
        dan.DeviceFamilyId
    FROM PRD_DeviceAliasNames dan
    WHERE dan.DeviceId = @DeviceId
    ORDER BY dan.AliasId;

    -- Result Set 3: Device Label Information
    SELECT 
        dli.LabelId,
        dli.DeviceId,
        dli.LabelName,
        dli.LabelDetails,
        dli.IsActive
    FROM PRD_DeviceLabelInfo dli
    WHERE dli.DeviceId = @DeviceId
    AND dli.IsActive = 1
    ORDER BY dli.LabelId;

    -- Result Set 4: Restricted Countries (if RestrictedCountriesToShipId contains comma-separated IDs)
    SELECT 
        c.CountryId,
        c.CountryName
    FROM Country c
    INNER JOIN PRD_Device d ON d.DeviceId = @DeviceId
    WHERE d.RestrictedCountriesToShipId IS NOT NULL
    AND d.RestrictedCountriesToShipId != ''
    AND d.RestrictedCountriesToShipId != '-1'
    AND d.RestrictedCountriesToShipId != '0'
    AND CHARINDEX(CAST(c.CountryId AS VARCHAR), d.RestrictedCountriesToShipId) > 0
    ORDER BY c.CountryName;

END
GO

