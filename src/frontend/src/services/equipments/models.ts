export const equipmentUrlPart: string = 'Equipments';

export type AddEquipmentDto = {
    name: string;
    additionalInfo: string;
    count: number;
}

export type Equipment = {
    id: string;
    name: string;
    additionalInfo: string;
    count: number;
    assignedPeopleCount: number;
}

export type EquipmentsFilterOptions = {
    name: string;
}

export type EditEquipmentDto = {
    equipmentId: string;
    name: string;
    count: number;
}

export type ReservationEquipmentDto = {
    equipmentId: string;
    employeeReservations?: ReservationEquipmentEmployeeDto[];
    dateFrom: Date;
}

export type ReservationEquipmentEmployeeDto = {
    employeeId: string;
    count: number;
    name: string;
    isNew: boolean;
}
export type EquipmentDetails = {
    id: string;
    name: string;
    additionalInfo: string;
    count: number;
    assignedPeopleCount: number;
    employees: EmployeeForEquipmentDetailsDto[];
    reservationsHistory: EquipmentReservationHistoryDto[];
}

export type EmployeeForEquipmentDetailsDto = {
    id: string;
    name: string;
    surname: string;
    equipmentNames: string[];
    count: number;
}

export type EquipmentReservationHistoryDto = {
    employeeId: string;
    employeeName: string;
    employeeSurname: string;
    reservationStart: Date;
    reservationEnd: Date;
    equipmentId: string;
    count: number;
}

export type EmployeeEquipmentDetailDto = {
    equipmentId: string;
    equipmentName: string;
    count: number;
}
