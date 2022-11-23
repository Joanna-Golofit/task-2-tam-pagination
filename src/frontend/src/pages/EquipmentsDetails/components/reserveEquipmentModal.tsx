import { useFormik } from 'formik';
import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useDispatch, useSelector } from 'react-redux';
import { Button, DropdownItemProps, DropdownProps, Form, Header, InputOnChangeData, Modal, Table } from 'semantic-ui-react';
import { ReservationEquipmentEmployeeDto } from '../../../services/equipments/models';
import { AppState } from '../../../store';
import { closeReservationEquipmentModalAction, reserveEquipmentDataAction } from '../../../store/equipmentDetails/actions';
import { setLoadingAction } from '../../../store/global/actions';

const ReserveEquipmentModal: React.FC = () => {
  const { t } = useTranslation();
  const dispatch = useDispatch();

  const { users } = useSelector((state: AppState) => state.users);
  const { isReserveEquipmentModalOpen: isModalOpen, equipmentDetailsResponse: equipmentDetails } = useSelector((state: AppState) => state.equipmentDetails);

  const [employeesOptions, setEmployeesOptions] = useState<DropdownItemProps[]>([]);
  const [employees, setEmployees] = useState<ReservationEquipmentEmployeeDto[]>([]);
  const [isFetchingEmployees, setIsFetchingEmployees] = useState(false);
  const [errorMaximumNumberOfEmployees, setErrorMaximumNumberOfEmployees] = useState(null);
  const [usedEquipments, setNumberOfUsedEquipments] = useState(0);

  const formik = useFormik({
    initialValues: {
      employeeIds: [] as string[],
      employeeReservationts: [] as ReservationEquipmentEmployeeDto[],
    },
    onSubmit: (values) => {
      dispatch(setLoadingAction(true));
      dispatch(reserveEquipmentDataAction({ equipmentId: equipmentDetails.id, dateFrom: new Date(), employeeReservations: values.employeeReservationts }));
    },
  });

  useEffect(() => {
    if (!isModalOpen) {
      return;
    }

    setErrorMaximumNumberOfEmployees(null);
    setIsFetchingEmployees(true);
    // TODO: should be refactored to use redux-observable flow
    const options = users.map((tl) => ({
      key: tl.id,
      text: tl.title,
      value: tl.id,
    }));
    options.sort((opA, opB) => (opA.text > opB.text ? 1 : -1));
    setEmployeesOptions(options);
    setIsFetchingEmployees(false);
    const employeesFromEquipments = equipmentDetails.employees.map((e) => ({ employeeId: e.id, name: `${e.name} ${e.surname}`, count: e.count, isNew: false }));
    setEmployees(employeesFromEquipments);
    const employeesCount = employeesFromEquipments.length > 0 ?
      employeesFromEquipments.map((e) => e.count).reduce((accumulator, curr) => accumulator + curr) : 0;
    setNumberOfUsedEquipments(employeesCount);

    formik.setValues({
      employeeIds: equipmentDetails.employees.map((e) => e.id),
      employeeReservationts: employeesFromEquipments,
    });
  }, [isModalOpen]);

  const submitForm = () => {
    formik.handleSubmit();
  };

  const onModalClose = () => {
    dispatch(closeReservationEquipmentModalAction());
  };

  const changeEmployeesHandler = (_: any, data: DropdownProps) => {
    const employeeIds = data.value as string[];
    const emps = [] as ReservationEquipmentEmployeeDto[];

    employeeIds.forEach((id) => {
      const empOption = data.options?.find((option: DropdownItemProps) => option.value === id);
      if (empOption) {
        const emp = { employeeId: id, name: empOption.text as string, count: 1, isNew: true };
        const existEmpl = employees.find((e: ReservationEquipmentEmployeeDto) => e.employeeId === id);
        if (existEmpl) {
          emp.count = existEmpl?.count;
          emp.isNew = existEmpl.isNew;
        }
        emps.push(emp);
      }
    });

    if (employeeIds.length < employees.length) {
      const employeeToDelated = [...employees.filter((empl) => !employeeIds.some((emplId) => empl.employeeId === emplId))];
      employeeToDelated.forEach((empl) => {
        if (!empl.isNew) {
          const emp = { employeeId: empl.employeeId, name: empl.name, count: 0, isNew: empl.isNew };
          emps.push(emp);
        }
      });
    }

    formik.setValues({ employeeIds, employeeReservationts: emps });
    setAndValidateEmployees(emps);
  };

  const setErrors = (maxCount: number | undefined) => {
    setErrorMaximumNumberOfEmployees(maxCount && equipmentDetails.count < maxCount ?
      t('equipmentDetails.valueMustBeLessOrEqualToAvailable') :
      null);
  };

  const handleOnChange = (data: InputOnChangeData, employee: ReservationEquipmentEmployeeDto) => {
    const empls = [...employees];

    const empl = empls.find((ttt: ReservationEquipmentEmployeeDto) => ttt.employeeId === employee.employeeId);
    if (empl) {
      empl.count = Number(data.value);
      formik.setValues({ employeeIds: formik.values.employeeIds, employeeReservationts: empls });
      setAndValidateEmployees(empls);
    }
  };

  const setAndValidateEmployees = (empls: ReservationEquipmentEmployeeDto[]) => {
    setEmployees(empls);
    const countOfUsed = empls.length > 0 ? empls.map((e) => e.count).reduce((accumulator, curr) => accumulator + curr) : 0;
    setNumberOfUsedEquipments(countOfUsed);
    setErrors(countOfUsed);
  };

  const rows = (
    employees?.length !== 0 ?
      employees.map(((employee) => (
        <Table.Row key={employee.employeeId}>
          <Table.Cell>
            <b>{employee.name}</b>
          </Table.Cell>
          <Table.Cell>
            <Form.Input
              id={`employeeCount-${employee.employeeId}`}
              type="number"
              value={employee.count}
              min={0}
              onChange={(e, data) => handleOnChange(data, employee)}
            />
          </Table.Cell>
        </Table.Row>
      ))) : (
        <Table.Row>
          <Table.Cell colSpan={4} textAlign="center"><i>{t('common.noResultsFilters')}</i></Table.Cell>
        </Table.Row>
      )
  );

  return (
    <Modal open={isModalOpen} onClose={onModalClose} size="tiny">
      <Modal.Header> {t('equipmentDetails.modalReservationHeader')}</Modal.Header>
      <Modal.Content>
        <Header as="h5">
          {`${t('equipmentDetails.availableQuantity')}: ${equipmentDetails.count}`}
        </Header>
        <Header as="h5">
          {`${t('equipmentDetails.usedEquipments')}: ${usedEquipments}`}
        </Header>
        <Form>
          <Form.Select
            name="employeeIds"
            clearable
            multiple
            search
            selection
            options={employeesOptions}
            loading={isFetchingEmployees}
            onChange={changeEmployeesHandler}
            value={formik.values.employeeIds}
            error={errorMaximumNumberOfEmployees}
          />
        </Form>
        <Table color="brown" unstackable>
          <Table.Header>
            <Table.Row>
              <Table.HeaderCell>{t('equipmentDetails.activeUsed')}</Table.HeaderCell>
              <Table.HeaderCell width={1} />
            </Table.Row>
          </Table.Header>
          <Table.Body>
            {!equipmentDetails.employees ? (
              <Table.Row>
                <Table.Cell textAlign="center" colSpan="5">
                  {t('equipmentDetails.noUsed')}
                </Table.Cell>
              </Table.Row>
            ) : rows}
          </Table.Body>
        </Table>
      </Modal.Content>
      <Modal.Actions>
        <Button
          size="tiny"
          disabled={errorMaximumNumberOfEmployees !== null}
          onClick={submitForm}
        >
          {t('equipmentDetails.reservation')}
        </Button>
      </Modal.Actions>
    </Modal>
  );
};

export default ReserveEquipmentModal;
