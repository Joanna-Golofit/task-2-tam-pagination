namespace TeamsAllocationManager.Dtos.Enums;

public enum ErrorCodes
{
	NoError = 0,
	AccessDenied,
	RoomNotExist,
	CouldNotAssignToDesk,
	CouldNotRemoveFromDesk,
	CouldNotReleaseRoomsDesks,
	ProjectDoesNotExist,
	DeskDoesNotExist,
	UnknownError,
	ServerIsNotAccessible,
	UserNotExist,
	RoomIsEmpty
}
