CREATE OR ALTER PROCEDURE GetAllCurrentTasks
@ProjectKey NVARCHAR(100)
AS   
SET NOCOUNT ON
BEGIN

	SELECT 
		DT.UserId,
		DT.UserName,
		DT.TaskUniqueId,
		DT.TaskSeq,
		DT.Catagory,
		DT.TaskName,
		DT.SubProject,
		DT.Network,
		DT.Status,
		0 AS LastDayWork,
		DT.TodayDayWork,
		DT.ItemToDiscuss,
		DT.EHrsToday,
		'' AS EHrsLast,
		DT.MyComments,
		DT.ManagerComments,
		DT.Jira,
		CASE WHEN DT.IsApprove = 1 
			THEN ISNULL((SELECT SUM(SubTaskETA) FROM SubTasks ST WITH(NOLOCK) WHERE DT.TaskUniqueId = ST.TaskUniqueId AND ST.IsApprove = 1 AND ST.IsSelf = 1), 0)
			ELSE ISNULL((SELECT SUM(SubTaskETA) FROM SubTasks ST WITH(NOLOCK) WHERE DT.TaskUniqueId = ST.TaskUniqueId AND ST.IsSelf = 1), 0) END AS TotalETA,
		ISNULL((SELECT SUM(EntryHrs) FROM SubTaskWorkEntrys WE WITH(NOLOCK) WHERE DT.TaskUniqueId = WE.TaskUniqueId AND WE.UserId = DT.UserId), 0) AS UsedETA,
		ISNULL((SELECT SUM(EntryHrs) FROM SubTaskWorkEntrys WE WITH(NOLOCK) WHERE DT.TaskUniqueId = WE.TaskUniqueId AND WE.UserId <> DT.UserId), 0) AS OtherUsedETA,
		DT.IsApprove,
		DT.RNGuidId,
		DT.FixVersion,
		DT.MailTitled
	FROM dbo.DailyTasks DT WITH(NOLOCK)
	WHERE DT.ProjectKey = @ProjectKey AND IsAlive = 1
END
GO

CREATE OR ALTER PROCEDURE GetTodayWorkCurrentTasks
@ProjectKey NVARCHAR(100),
@DisplayDate DATE
AS   
SET NOCOUNT ON
BEGIN
	
	SELECT 
		DT.TaskUniqueId, U.FirstName, SUM(WE.EntryHrs) WorkHrs 
	FROM SubTaskWorkEntrys WE WITH(NOLOCK) 
	JOIN dbo.DailyTasks DT WITH(NOLOCK) ON (DT.TaskUniqueId = WE.TaskUniqueId)
	JOIN dbo.Users U WITH(NOLOCK) ON (U.UserId = WE.UserId)
	WHERE DT.ProjectKey = @ProjectKey AND WE.EntryDate = @DisplayDate AND IsAlive = 1
	GROUP BY DT.TaskUniqueId, U.FirstName
END
GO

CREATE OR ALTER PROCEDURE GetAllDoneTasks
@ProjectKey NVARCHAR(100)
AS   
SET NOCOUNT ON
BEGIN

	SELECT 
		DT.UserId,
		DT.UserName,
		DT.TaskUniqueId,
		DT.TaskSeq,
		DT.Catagory,
		DT.TaskName,
		DT.SubProject,
		DT.Network,
		DT.Status,
		DT.MyComments,
		DT.ManagerComments,
		DT.Jira,
		CASE WHEN DT.IsApprove = 1 
			THEN ISNULL((SELECT SUM(SubTaskETA) FROM SubTasks ST WITH(NOLOCK) WHERE DT.TaskUniqueId = ST.TaskUniqueId AND ST.IsApprove = 1 AND ST.IsSelf = 1), 0)
			ELSE ISNULL((SELECT SUM(SubTaskETA) FROM SubTasks ST WITH(NOLOCK) WHERE DT.TaskUniqueId = ST.TaskUniqueId AND ST.IsSelf = 1), 0) END AS TotalETA,
		ISNULL((SELECT SUM(EntryHrs) FROM SubTaskWorkEntrys WE WITH(NOLOCK) WHERE DT.TaskUniqueId = WE.TaskUniqueId AND WE.UserId = DT.UserId), 0) AS UsedETA,
		ISNULL((SELECT SUM(EntryHrs) FROM SubTaskWorkEntrys WE WITH(NOLOCK) WHERE DT.TaskUniqueId = WE.TaskUniqueId AND WE.UserId <> DT.UserId), 0) AS OtherUsedETA,
		ISNULL(R.RN, '') AS RNName,
		DT.FixVersion,
		DT.MailTitled
	FROM dbo.DailyTasks DT WITH(NOLOCK)
	LEFT JOIN dbo.RNAndFeatureList R WITH(NOLOCK) ON (R.GuidId = DT.RNGuidId)
	WHERE DT.ProjectKey = @ProjectKey AND IsAlive = 0
END
GO

CREATE OR ALTER PROCEDURE GetCurrentTaskById
@TaskUniqueId UNIQUEIDENTIFIER
AS   
SET NOCOUNT ON
BEGIN
	SELECT 
		DT.UserId,
		DT.UserName,
		DT.TaskUniqueId,
		DT.TaskSeq,
		DT.Catagory,
		DT.TaskName,
		DT.SubProject,
		DT.Network,
		DT.Status,
		0 AS LastDayWork,
		DT.TodayDayWork,
		DT.ItemToDiscuss,
		DT.EHrsToday,
		'' AS EHrsLast,
		DT.MyComments,
		DT.ManagerComments,
		DT.Jira,
		CASE WHEN DT.IsApprove = 1 
			THEN ISNULL((SELECT SUM(SubTaskETA) FROM SubTasks ST WITH(NOLOCK) WHERE ST.TaskUniqueId = @TaskUniqueId AND ST.IsApprove = 1 AND ST.IsSelf = 1), 0)
			ELSE ISNULL((SELECT SUM(SubTaskETA) FROM SubTasks ST WITH(NOLOCK) WHERE ST.TaskUniqueId = @TaskUniqueId AND ST.IsSelf = 1), 0) END AS TotalETA,
		ISNULL((SELECT SUM(EntryHrs) FROM SubTaskWorkEntrys WE WITH(NOLOCK) WHERE WE.TaskUniqueId = @TaskUniqueId AND WE.UserId = DT.UserId), 0) AS UsedETA,
		ISNULL((SELECT SUM(EntryHrs) FROM SubTaskWorkEntrys WE WITH(NOLOCK) WHERE WE.TaskUniqueId = @TaskUniqueId AND WE.UserId <> DT.UserId), 0) AS OtherUsedETA,
		DT.IsApprove,
		DT.RNGuidId,
		DT.FeatureName,
		DT.FixVersion,
		DT.RNComments,
		DT.MailTitled
	FROM dbo.DailyTasks DT WITH(NOLOCK)
	WHERE DT.TaskUniqueId = @TaskUniqueId
END
GO

CREATE OR ALTER PROCEDURE GetTodayWorkCurrentTaskById
@TaskUniqueId UNIQUEIDENTIFIER,
@DisplayDate DATE
AS   
SET NOCOUNT ON
BEGIN
	
	SELECT 
		DT.TaskUniqueId, U.FirstName, ISNULL(SUM(WE.EntryHrs), 0) WorkHrs 
	FROM SubTaskWorkEntrys WE WITH(NOLOCK) 
	JOIN dbo.DailyTasks DT WITH(NOLOCK) ON (DT.TaskUniqueId = WE.TaskUniqueId)
	JOIN dbo.Users U WITH(NOLOCK) ON (U.UserId = WE.UserId)
	WHERE DT.TaskUniqueId = @TaskUniqueId AND WE.EntryDate = @DisplayDate
	GROUP BY DT.TaskUniqueId, U.FirstName
END
GO

CREATE OR ALTER PROCEDURE AddSubTask
@CurrentUser_UserId	BIGINT,
@SubTaskUniqueId	UNIQUEIDENTIFIER,
@TaskUniqueId		UNIQUEIDENTIFIER,
@SubTaskSeq			INT,
@SubTaskName		NVARCHAR(MAX),
@Remark				NVARCHAR(MAX),
@SubTaskETA			DECIMAL(5, 2),
@Status				NVARCHAR(200),
@IsColour			BIT,
@IsSelf				BIT
AS   
SET NOCOUNT ON
BEGIN
	DECLARE @Log NVARCHAR(MAX),
		@TotalETA DECIMAL(5, 2) = 0

	INSERT INTO dbo.SubTasks 
	(
		SubTaskUniqueId,
		TaskUniqueId,
		SubTaskSeq,
		SubTaskName,
		Remark,
		SubTaskETA,
		Status,
		IsColour,
		UserId,
		AssignedAt,
		IsApprove,
		IsSelf
	)
	VALUES
	(
		@SubTaskUniqueId,
		@TaskUniqueId,
		@SubTaskSeq,
		@SubTaskName,
		@Remark,
		@SubTaskETA,
		@Status,
		@IsColour,
		@CurrentUser_UserId,
		GETDATE(),
		0,
		@IsSelf
	)

	SET @Log = CASE WHEN @IsSelf = 1 THEN 'New SubTask Added - SubTaskSeq :- "' ELSE 'New Other SubTask Added - SubTaskSeq :- "' END
	SET @Log = @Log + CAST(@SubTaskSeq AS NVARCHAR(15)) + '"' + CHAR(13) + CHAR(10) + 'SubTaskName - "' + @SubTaskName + '"' + CHAR(13) + CHAR(10) + 'Remark - "' + @Remark + '"' + CHAR(13) + CHAR(10) 
		+ 'SubTaskETA - "' + CAST(@SubTaskETA AS NVARCHAR(15)) + '"' + CHAR(13) + CHAR(10) + 'Status - "' + @Status + '"'

	INSERT INTO dbo.DailyTasksLogs (TaskId, Log, UserId, Date)
	VALUES (@TaskUniqueId, @Log, @CurrentUser_UserId, GETDATE())

	--IF(@IsSelf = 1)
	--BEGIN
	--	IF EXISTS(SELECT TOP 1 1 FROM dbo.DailyTasks DT WITH(NOLOCK) WHERE DT.TaskUniqueId = @TaskUniqueId AND DT.IsApprove = 0)
	--	BEGIN
	--		SELECT @TotalETA = SUM(SubTaskETA) FROM dbo.SubTasks ST WITH(NOLOCK) WHERE ST.TaskUniqueId = @TaskUniqueId AND ST.IsSelf = 1
	--		SET @TotalETA = ISNULL(@TotalETA, 0)
	--	END
	--END

	IF EXISTS(SELECT TOP 1 1 FROM dbo.DailyTasks DT WITH(NOLOCK) WHERE DT.TaskUniqueId = @TaskUniqueId AND DT.IsApprove = 0)
	BEGIN
		SELECT @TotalETA = SUM(SubTaskETA) FROM dbo.SubTasks ST WITH(NOLOCK) WHERE ST.TaskUniqueId = @TaskUniqueId AND ST.IsSelf = 1
	END
	ELSE
	BEGIN
		SELECT @TotalETA = SUM(SubTaskETA) FROM dbo.SubTasks ST WITH(NOLOCK) WHERE ST.TaskUniqueId = @TaskUniqueId AND ST.IsApprove = 1 AND ST.IsSelf = 1
	END
	SET @TotalETA = ISNULL(@TotalETA, 0)

	SELECT @TotalETA AS TotalETA
END
GO


CREATE OR ALTER PROCEDURE UpdateSubTask
@CurrentUser_UserId	BIGINT,
@SubTaskUniqueId	UNIQUEIDENTIFIER,
@SubTaskSeq			INT,
@SubTaskName		NVARCHAR(MAX),
@Remark				NVARCHAR(MAX),
@SubTaskETA			DECIMAL(5, 2),
@Status				NVARCHAR(200),
@IsColour			BIT
AS   
SET NOCOUNT ON
BEGIN
	DECLARE @Log			NVARCHAR(MAX) = '',
		@OldSubTaskSeq		INT,
		@OldSubTaskName		NVARCHAR(MAX),
		@OldRemark			NVARCHAR(MAX),
		@OldSubTaskETA		DECIMAL(5, 2),
		@OldStatus			NVARCHAR(200),
	
		@TotalETA			DECIMAL(5, 2) = 0,
		@IsApprove			BIT,
		@IsSelf				BIT,
		@TaskUniqueId		UNIQUEIDENTIFIER,
		@Colour				INT = 0

	SELECT TOP 1 
			@TaskUniqueId = TaskUniqueId,
			@OldSubTaskSeq = SubTaskSeq,
			@OldSubTaskName = SubTaskName,
			@OldRemark = Remark,
			@OldSubTaskETA = SubTaskETA,
			@OldStatus = Status,
			@IsApprove = IsApprove,
			@IsSelf = IsSelf
	FROM dbo.SubTasks WITH(NOLOCK) WHERE SubTaskUniqueId = @SubTaskUniqueId

	IF(@IsApprove = 0 AND @IsSelf = 1)
	BEGIN
		UPDATE dbo.SubTasks SET 
			SubTaskSeq  = @SubTaskSeq,
			SubTaskName = @SubTaskName,
			Remark		= @Remark,
			SubTaskETA  = @SubTaskETA,
			Status		= @Status
		WHERE SubTaskUniqueId = @SubTaskUniqueId

		SET @Colour = 1
	END
	ELSE
	BEGIN
		UPDATE dbo.SubTasks SET 
			SubTaskSeq  = @SubTaskSeq,
			SubTaskName = @SubTaskName,
			Remark		= @Remark,
			Status		= @Status
		WHERE SubTaskUniqueId = @SubTaskUniqueId
	END

	IF(@IsApprove = 0)
	BEGIN
		SELECT @TotalETA = SUM(SubTaskETA) FROM dbo.SubTasks ST WITH(NOLOCK) WHERE ST.TaskUniqueId = @TaskUniqueId AND ST.IsSelf = 1
	END
	ELSE
	BEGIN
		SELECT @TotalETA = SUM(SubTaskETA) FROM dbo.SubTasks ST WITH(NOLOCK) WHERE ST.TaskUniqueId = @TaskUniqueId AND ST.IsApprove = 1 AND ST.IsSelf = 1
	END
	SET @TotalETA = ISNULL(@TotalETA, 0)

	IF(@OldSubTaskSeq <> @SubTaskSeq)
	BEGIN
		SET @Log = @Log + '' + CHAR(13) + CHAR(10) + 'SubTaskSeq - From "' + CAST(@OldSubTaskSeq AS NVARCHAR(15)) + '" To "'  + CAST(@SubTaskSeq AS NVARCHAR(15)) + '"'
	END
	IF(@OldSubTaskName <> @SubTaskName)
	BEGIN
		SET @Log = @Log + '' + CHAR(13) + CHAR(10) + 'SubTaskName - From "' + @OldSubTaskName + '" To "'  + @SubTaskName + '"'
	END
	IF(@OldRemark <> @Remark)
	BEGIN
		SET @Log = @Log + '' + CHAR(13) + CHAR(10) + 'Remark - From "' + @OldRemark + '" To "'  + @Remark + '"'
	END
	IF(@IsApprove = 0)
	BEGIN
		IF(@OldSubTaskETA <> @SubTaskETA)
		BEGIN
			SET @Log = @Log + '' + CHAR(13) + CHAR(10) + 'SubTaskETA - From "' + CAST(@OldSubTaskETA AS NVARCHAR(15)) + '" To "'  + CAST(@SubTaskETA AS NVARCHAR(15)) + '"'
		END
	END
	IF(@OldStatus <> @Status)
	BEGIN
		SET @Log = @Log + '' + CHAR(13) + CHAR(10) + 'Status - From "' + @OldStatus + '" To "'  + @Status + '"'
	END
	
	IF(@Log <> '')
	BEGIN
		SET @Log = CASE WHEN @IsSelf = 1 THEN 'SubTask changes Details : ' ELSE 'Other SubTask changes Details :' END + @Log

		INSERT INTO dbo.DailyTasksLogs (TaskId, Log, UserId, Date, Colour)
		VALUES (@TaskUniqueId, @Log, @CurrentUser_UserId, GETDATE(), @Colour)

	END
	SELECT @TotalETA AS TotalETA
END
GO


CREATE OR ALTER PROCEDURE AddSubTaskWorkEntry
@CurrentUser_UserId	BIGINT,
@SubTaskUniqueId	UNIQUEIDENTIFIER,
@TaskUniqueId		UNIQUEIDENTIFIER,
@EntryDate			DATE,
@EntryHrs			DECIMAL(5, 2)
AS   
SET NOCOUNT ON
BEGIN
	DECLARE @UserId				BIGINT,
			@IsSelf				BIT,
			@IsError			BIT = 0,
			@ErrorMessage		NVARCHAR(200) = '',
			@OldEntryHrs		DECIMAL(5, 2),
			@WorkEntryUniqueId  UNIQUEIDENTIFIER = NEWID(),
			@UsedETA			DECIMAL(5, 2) = 0,
			@OtherUsedETA		DECIMAL(5, 2) = 0
	
	SELECT TOP 1 @UserId = UserId FROM dbo.DailyTasks DT WITH(NOLOCK) WHERE DT.TaskUniqueId = @TaskUniqueId

	SELECT TOP 1 @IsSelf = IsSelf FROM dbo.SubTasks WITH(NOLOCK) WHERE SubTaskUniqueId = @SubTaskUniqueId



	IF(@IsSelf = 1)
	BEGIN
		IF(@UserId <> @CurrentUser_UserId)
		BEGIN
			SET @IsError = 1
			SET @ErrorMessage = 'This is assigned task, please fill time in other user subtask'
		END
	END
	ELSE
	BEGIN
		IF(@UserId = @CurrentUser_UserId)
		BEGIN
			SET @IsError = 1
			SET @ErrorMessage = 'This is other user subtask, please fill time in your subtask'
		END
	END

	IF(@IsError = 0)
	BEGIN
		
		SELECT TOP 1 @OldEntryHrs = EntryHrs, @WorkEntryUniqueId = WorkEntryUniqueId FROM dbo.SubTaskWorkEntrys WITH(NOLOCK) WHERE SubTaskUniqueId = @SubTaskUniqueId AND EntryDate = @EntryDate
		IF(@OldEntryHrs IS NULL)
		BEGIN
			INSERT INTO dbo.SubTaskWorkEntrys (WorkEntryUniqueId, SubTaskUniqueId, TaskUniqueId, EntryDate, EntryHrs, AssignedAt, UserId)
			SELECT @WorkEntryUniqueId, @SubTaskUniqueId, @TaskUniqueId, @EntryDate, @EntryHrs, GETDATE(), @CurrentUser_UserId
		END
		ELSE
		BEGIN
			SET @EntryHrs = @OldEntryHrs + ISNULL(@EntryHrs, 0)
			UPDATE dbo.SubTaskWorkEntrys SET EntryHrs = @EntryHrs WHERE WorkEntryUniqueId = @WorkEntryUniqueId
		END

		SELECT @UsedETA = SUM(EntryHrs) FROM SubTaskWorkEntrys WE WITH(NOLOCK) WHERE WE.TaskUniqueId = @TaskUniqueId AND WE.UserId = @UserId
		SET @UsedETA = ISNULL(@UsedETA, 0)

		SELECT @OtherUsedETA = SUM(EntryHrs) FROM SubTaskWorkEntrys WE WITH(NOLOCK) WHERE WE.TaskUniqueId = @TaskUniqueId AND WE.UserId <> @UserId
		SET @OtherUsedETA = ISNULL(@OtherUsedETA, 0)
	END

	SELECT @IsError AS IsError, @ErrorMessage AS ErrorMessage, @WorkEntryUniqueId AS WorkEntryUniqueId, @EntryHrs AS EntryHrs, @UsedETA AS UsedETA, @OtherUsedETA AS OtherUsedETA

END
GO

CREATE OR ALTER PROCEDURE UpdateSubTaskWorkEntry
@CurrentUser_UserId	BIGINT,
@WorkEntryUniqueId	UNIQUEIDENTIFIER,
@EntryHrs			DECIMAL(5, 2)
AS   
SET NOCOUNT ON
BEGIN
	DECLARE @UserId				BIGINT,
			@IsError			BIT = 0,
			@SubTaskUniqueId	UNIQUEIDENTIFIER,
			@UsedETA			DECIMAL(5, 2),
			@OtherUsedETA		DECIMAL(5, 2) = 0,
			@TaskUniqueId		UNIQUEIDENTIFIER


	SELECT TOP 1 @TaskUniqueId = TaskUniqueId, @SubTaskUniqueId = SubTaskUniqueId, @UserId = UserId FROM dbo.SubTaskWorkEntrys WITH(NOLOCK) WHERE WorkEntryUniqueId = @WorkEntryUniqueId

	IF(@UserId = @CurrentUser_UserId)
	BEGIN

		IF(@EntryHrs > 0)
		BEGIN
			UPDATE dbo.SubTaskWorkEntrys SET EntryHrs = @EntryHrs WHERE WorkEntryUniqueId = @WorkEntryUniqueId
		END
		ELSE
		BEGIN
			DELETE FROM dbo.SubTaskWorkEntrys WHERE WorkEntryUniqueId = @WorkEntryUniqueId
		END

		SELECT @UsedETA = SUM(EntryHrs) FROM SubTaskWorkEntrys WE WITH(NOLOCK) WHERE WE.TaskUniqueId = @TaskUniqueId AND WE.UserId = @UserId
		SET @UsedETA = ISNULL(@UsedETA, 0)

		SELECT @OtherUsedETA = SUM(EntryHrs) FROM SubTaskWorkEntrys WE WITH(NOLOCK) 
		JOIN dbo.DailyTasks DT WITH(NOLOCK) ON (DT.TaskUniqueId = WE.TaskUniqueId) WHERE WE.TaskUniqueId = @TaskUniqueId AND DT.UserId <> @UserId
		SET @OtherUsedETA = ISNULL(@OtherUsedETA, 0)
	END
	ELSE
	BEGIN
		SET @IsError = 1
	END

	SELECT @IsError AS IsError, @EntryHrs AS EntryHrs, @UsedETA AS UsedETA, @OtherUsedETA AS OtherUsedETA
END
GO

CREATE OR ALTER PROCEDURE ApproveETA
@CurrentUser_UserId	BIGINT,
@UniqueId			UNIQUEIDENTIFIER,
@IsAll				BIT
AS   
SET NOCOUNT ON
BEGIN
	DECLARE @IsError			BIT = 0,
			@ErrorMessage		NVARCHAR(200) = '',
			@TotalETA			DECIMAL(5, 2) = 0,
			@TaskUniqueId		UNIQUEIDENTIFIER
	

	IF EXISTS (SELECT TOP 1 1 FROM dbo.UserRoles WITH(NOLOCK) WHERE RoleId IN (101, 102) AND UserId = @CurrentUser_UserId)
	BEGIN
		IF(@IsAll = 1)
		BEGIN
			UPDATE dbo.SubTasks SET IsApprove = 1 WHERE TaskUniqueId = @UniqueId
			UPDATE dbo.DailyTasks SET IsApprove = 1 WHERE TaskUniqueId = @UniqueId

			SELECT @TotalETA = SUM(SubTaskETA) FROM SubTasks ST WITH(NOLOCK) WHERE ST.TaskUniqueId = @UniqueId AND ST.IsSelf = 1
		END
		ELSE
		BEGIN
			UPDATE dbo.SubTasks SET IsApprove = 1 WHERE SubTaskUniqueId = @UniqueId

			SELECT TOP 1 @TaskUniqueId = TaskUniqueId FROM dbo.SubTasks  WITH(NOLOCK) WHERE SubTaskUniqueId = @UniqueId

			UPDATE dbo.DailyTasks SET IsApprove = 1 WHERE TaskUniqueId = @TaskUniqueId

			SELECT @TotalETA = SUM(SubTaskETA) FROM SubTasks ST WITH(NOLOCK) WHERE ST.TaskUniqueId = @TaskUniqueId AND ST.IsSelf = 1
		END
		SET @TotalETA = ISNULL(@TotalETA, 0)

	END
	ELSE
	BEGIN
		SET @IsError = 1
		SET @ErrorMessage = 'You can not approve this task ETA, only manager have permission to do.'
	END

	SELECT @IsError AS IsError, @ErrorMessage AS ErrorMessage, @TotalETA AS TotalETA
END
GO

--==================================================================================================================
--GetTasksToCopy 'cpp', 1
CREATE OR ALTER PROCEDURE GetTasksToCopy
@ProjectKey NVARCHAR(100),
@IsAlive	BIT = 1
AS   
SET NOCOUNT ON
BEGIN

	SELECT
		U.FirstName,
		DT.TaskUniqueId,
		DT.TaskSeq,
		DT.TaskName,
		DT.SubProject,
		DT.Network,
		DT.Status
	FROM dbo.DailyTasks DT WITH(NOLOCK)
	JOIN dbo.Users U WITH(NOLOCK) ON (U.UserName = DT.UserName)
	WHERE DT.ProjectKey = @ProjectKey AND IsAlive = @IsAlive
END
GO


--GetSubTasksToCopy '5DE7E396-0FE9-439A-8BC5-732BE7641297'
CREATE OR ALTER PROCEDURE GetSubTasksToCopy
@TaskUniqueId	UNIQUEIDENTIFIER
AS   
SET NOCOUNT ON
BEGIN

	SELECT
		ST.SubTaskSeq,
		ST.SubTaskName,
		ST.SubTaskETA
	FROM dbo.SubTasks ST WITH(NOLOCK) WHERE ST.TaskUniqueId = @TaskUniqueId AND ST.IsSelf = 1
END
GO


CREATE OR ALTER PROCEDURE GetAllTasksByRNGuidId
@ProjectKey NVARCHAR(100),
@GuidId	UNIQUEIDENTIFIER
AS   
SET NOCOUNT ON
BEGIN

	SELECT 
		DT.TaskUniqueId,
		DT.Jira,
		DT.FeatureName,
		DT.FixVersion,
		DT.RNComments,
		U.FirstName,
		DT.IsAlive
	FROM dbo.DailyTasks DT WITH(NOLOCK)
	JOIN dbo.Users U WITH(NOLOCK) ON (U.UserName = DT.UserName)
	WHERE DT.ProjectKey = @ProjectKey AND DT.RNGuidId = @GuidId
END
GO