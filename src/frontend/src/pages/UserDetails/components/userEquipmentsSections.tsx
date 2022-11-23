import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useDispatch } from 'react-redux';
import { Form, Responsive, Table } from 'semantic-ui-react';
import styles from './userEquipmentsSections.module.scss';
import { LAPTOP_MEDIA_WIDTH } from '../../../globalConstants';
import getEquipmentsForEmployeeApiService from '../../../services/equipments/getEquipmentsForEmployeeApiService';
import { EmployeeEquipmentDetailDto } from '../../../services/equipments/models';
import { setLoadingAction } from '../../../store/global/actions';

interface Props {
  userId: string;
}

const UserEquipmentsSection: React.FC<Props> = (props) => {
  const { t } = useTranslation();
  const { userId } = props;
  const dispatch = useDispatch();
  const [equipments, setEquipments] = useState([] as Array<EmployeeEquipmentDetailDto>);

  useEffect(() => {
    if (userId) {
      dispatch(setLoadingAction(true));
      getEquipmentsForEmployeeApiService(userId).subscribe((returnEquipments) => {
        dispatch(setLoadingAction(false));
        setEquipments(returnEquipments);
      });
    }
  }, [dispatch, userId]);

  const rows = (
    equipments?.length !== 0 ?
      equipments.map((equipment: EmployeeEquipmentDetailDto) => (
        <Table.Row key={equipment.equipmentId}>
          <Table.Cell>
            <b>{`${equipment.equipmentName}`}</b>
          </Table.Cell>
          <Table.Cell>
            <b>{`${equipment.count}`}</b>
          </Table.Cell>
        </Table.Row>
      )) :
      (
        <Table.Row>
          <Table.Cell colSpan={4} textAlign="center"><i>{t('common.noResultsFilters')}</i></Table.Cell>
        </Table.Row>
      )
  );
  return (
    <>
      <Form>
        <div className={styles.reservationHeader}>
          <h3>{t('userDetails.equipments')}</h3>
        </div>
        <Table compact color="orange" unstackable>
          <Table.Header>
            <Responsive as={Table.Row} minWidth={LAPTOP_MEDIA_WIDTH}>
              <Table.HeaderCell width={12}>{t('userDetails.equipmentsName')}</Table.HeaderCell>
              <Table.HeaderCell>{t('userDetails.numberOfEquipments')}</Table.HeaderCell>
            </Responsive>
            <Responsive as={Table.Row} maxWidth={LAPTOP_MEDIA_WIDTH - 1}>
              <Table.HeaderCell>{t('userDetails.equipmentsName')}</Table.HeaderCell>
              <Table.HeaderCell>{t('userDetails.numberOfEquipments')}</Table.HeaderCell>
            </Responsive>
          </Table.Header>
          <Table.Body>
            {(equipments?.length || 0) === 0 ? (
              <Table.Row>
                <Table.Cell colSpan={4} textAlign="center"><i>{t('common.noResultsFilters')}</i></Table.Cell>
              </Table.Row>
            ) : rows}
          </Table.Body>
        </Table>
      </Form>
    </>
  );
};

export default UserEquipmentsSection;
