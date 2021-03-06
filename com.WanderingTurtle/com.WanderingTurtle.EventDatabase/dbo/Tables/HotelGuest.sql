﻿CREATE TABLE [dbo].[HotelGuest] (
    [HotelGuestID] INT           IDENTITY (100, 1) NOT NULL,
    [FirstName]    VARCHAR (50)  NOT NULL,
    [LastName]     VARCHAR (50)  NOT NULL,
    [Zip]          CHAR    (5)   NOT NULL,
    [Address1]     VARCHAR (255) NOT NULL,
    [Address2]     VARCHAR (255) NULL,
    [PhoneNumber]  VARCHAR (15)  NULL,
    [EmailAddress] VARCHAR (100) NULL,
	[Room]		   CHAR(4)			 NULL,
    [GuestPIN]	   CHAR(6)			 NULL, 
    [Active]       BIT           NOT NULL,
    CONSTRAINT [pk_HotelGuest] PRIMARY KEY CLUSTERED ([HotelGuestID] ASC)
) ON [PRIMARY];

GO
ALTER TABLE [dbo].[HotelGuest]
    ADD CONSTRAINT [fk_HotelGuest_CityState] FOREIGN KEY ([Zip]) REFERENCES [dbo].[CityState] ([Zip]);
GO
ALTER TABLE [dbo].[HotelGuest]
    ADD DEFAULT 1 FOR [Active];
GO
CREATE UNIQUE NONCLUSTERED INDEX [UniquePINExceptNulls]
ON [HotelGuest] ([GuestPIN])
WHERE [GuestPIN] IS NOT NULL
GO
CREATE UNIQUE NONCLUSTERED INDEX [UniqueRoomExceptNulls]
ON [HotelGuest] ([Room])
WHERE [Room] IS NOT NULL
GO