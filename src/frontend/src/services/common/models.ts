export type ListOptions = {
    pageNumber: number;
    pageSize: number;
}

export type PagedQueryDto = {
    pageNumber: number;
    pageSize: number;
}

export type PagedQueryWithFilterDto<T> = {
    pageNumber: number;
    pageSize: number;
    filters: T;
}

export type PagedResultDto<T> = {
    count: number;
    payload: T;
}

export enum ErrorCodes {
    NoError = 0,
    AccessDenied,
    RoomNotExist,
    UnknownError,
    ServerIsNotAccessible,
    CouldNotAddToDesk,
    CouldNotRemoveFromDesk,
    DeskDoesNotExist,
    ProjectDoesNotExist,
    CouldNotReleaseRoomsDesks,
    UserNotExist,
    CouldNotGetSummary
  }
