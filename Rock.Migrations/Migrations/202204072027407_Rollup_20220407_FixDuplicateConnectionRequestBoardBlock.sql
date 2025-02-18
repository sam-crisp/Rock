-- Script to correct duplicate ConnectionRequestBoard block type with the double-slash in the path
DECLARE @BlockEntityTypeId int = (SELECT [Id] FROM [EntityType] WHERE [Guid] = 'D89555CA-9AE4-4D62-8AF1-E5E463C1EF65')

DECLARE @ConnectionRequestBoardKeepId int = (SELECT [Id] FROM [BlockType] WHERE [Guid] = '28DBE708-E99B-4879-A64D-656C030D25B5')
DECLARE @ConnectionRequestBoardDeleteId int = (SELECT [Id] FROM [BlockType] WHERE [Guid] = '5F2FE25A-9D94-4B81-8783-EC32DD062913')

DECLARE @KeepAttributeId int
DECLARE @AttributeId int
DECLARE @AttributeValueId int
DECLARE @AttributeKey nvarchar(1000)

-- Check for any usage of the ConnectionRequestBoard block type ID that is going to be deleted
IF EXISTS (SELECT [Id] FROM [Block] WHERE [BlockTypeId] = @ConnectionRequestBoardDeleteId)
BEGIN
	-- Update block attribute values from the delete blocktypeid to the keep blocktypeid
	DECLARE attributeValueCursor CURSOR FOR
	SELECT av.[AttributeId], av.[Id] as AttributeValueId, a.[Key] as AttributeKey
	FROM [AttributeValue] av
	JOIN [Attribute] a on av.AttributeId = a.Id
	WHERE a.[EntityTypeId] = @BlockEntityTypeId 
		AND a.[EntityTypeQualifierColumn] = 'BlockTypeId' 
		AND a.[EntityTypeQualifierValue] = @ConnectionRequestBoardDeleteId
	
	OPEN attributeValueCursor
	FETCH NEXT FROM attributeValueCursor INTO @AttributeId, @AttributeValueId, @AttributeKey
	WHILE @@FETCH_STATUS = 0  
	BEGIN
		-- Get the ID of the attribute for the block we are going to keep
		SET @KeepAttributeId = (SELECT [Id]
		FROM [Attribute]
		WHERE [EntityTypeId] = @BlockEntityTypeId 
			AND [EntityTypeQualifierColumn] = 'BlockTypeId' 
			AND [EntityTypeQualifierValue] = @ConnectionRequestBoardKeepId
			AND [Key] = @AttributeKey)

		UPDATE [AttributeValue]
		SET [AttributeId] = @KeepAttributeId
		WHERE [Id] = @AttributeValueId

        -- Delete the old attribute
        DELETE [Attribute] WHERE [Id] = @AttributeId

		FETCH NEXT FROM attributeValueCursor INTO @AttributeId, @AttributeValueId, @AttributeKey
    END

	CLOSE attributeValueCursor
	DEALLOCATE attributeValueCursor

	-- Update any blocks using the ID that is going to be deleted with the one that is going to be kept
	UPDATE [Block] SET BlockTypeId = @ConnectionRequestBoardKeepId WHERE BlockTypeId = @ConnectionRequestBoardDeleteId
END

-- Correct the path in the BlockType that is going to be kept
UPDATE [BlockType] SET [Path] = REPLACE([Path], '//', '/') WHERE [Id] = @ConnectionRequestBoardKeepId

-- Delete the duplicate BlockType
DELETE FROM [BlockType] WHERE [Id] = @ConnectionRequestBoardDeleteId
