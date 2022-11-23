import React, { useMemo } from 'react';
import { Form, Button, Grid, GridColumnProps, DropdownItemProps, DropdownProps, InputOnChangeData } from 'semantic-ui-react';
import { useTranslation } from 'react-i18next';
import { useFormik } from 'formik';
import { useSelector, useDispatch } from 'react-redux';
import { GRID_COLUMN_COMPUTER, GRID_COLUMN_MOBILE, GRID_COLUMN_TABLET } from '../../../globalConstants';
import { AppState } from '../../../store';
import { resetDateFilterAction, setDateFilterAction } from '../../../store/hotDesks/actions';
import { HotDeskFilters } from '../../../services/hotDesks/models';
import styles from './filters.module.scss';
import Datepicker from '../../../components/Datepicker';

type ComponentProps = {
  onFilter: (filters: HotDeskFilters) => void,
}

const Filters: React.FC<ComponentProps> = (props) => {
  const { t } = useTranslation();
  const dispatch = useDispatch();
  const { filters, response, initFilters, initNonStdFilters } = useSelector((state: AppState) => state.hotDesks);
  const { loggedUserData } = useSelector((state: AppState) => state.global);

  const buildingOptions = useMemo(() =>
    response.buildings.map((b) => ({ key: b.id, text: b.name, value: b.id })),
  [response.buildings]);

  const floorOptions: DropdownItemProps[] = useMemo(() =>
    [...Array(response.maxFloor + 1).keys()].map((i) => ({ key: i, text: i, value: i })),
  [response.maxFloor]);

  const formik = useFormik({
    initialValues: {
      room: '',
      buildingIds: Array(0),
      floors: Array(0),
      freeHotDeskMin: undefined,
      freeHotDeskMax: undefined,
      startDate: filters.startDate,
      endDate: filters.endDate,
    },

    onSubmit: (values) => {
      const valuesDeepCopy = JSON.parse(JSON.stringify(values));

      if (valuesDeepCopy.buildingIds && values.buildingIds!.length === 0) {
        buildingOptions.forEach((item) => valuesDeepCopy.buildingIds.push(item.value));
      }

      if (valuesDeepCopy.floors && values.floors!.length === 0) {
        floorOptions.forEach((item) => valuesDeepCopy.floors.push(item.value));
      }

      props.onFilter({
        ...values,
        room: valuesDeepCopy.room,
        buildingIds: values.buildingIds?.length ? values.buildingIds : Array(0),
        floors: values.floors?.length ? values.floors : Array(0),
      });
    },
  });

  const columnCommonProps: GridColumnProps = {
    mobile: GRID_COLUMN_MOBILE, tablet: GRID_COLUMN_TABLET, computer: GRID_COLUMN_COMPUTER,
  };

  const updateDropdown = (_: React.SyntheticEvent<HTMLElement>, data: DropdownProps) => {
    formik.setFieldValue(data.name, data.value);
    formik.handleSubmit();
  };

  const updateValues = (_: React.ChangeEvent, data: InputOnChangeData) => {
    if (data.type === 'number') {
      formik.setFieldValue(data.name, Number(data.value) > 0 ? data.value : '0');
    } else {
      formik.setFieldValue(data.name, data.value);
    }
    formik.handleSubmit();
  };

  const updateStartDate = (event: any, data: any) => {
    if (data.value !== formik.values.startDate) {
      const startDate = data.value ? data.value : initFilters.startDate;
      let { endDate } = formik.values;

      formik.setFieldValue('startDate', startDate);

      if (startDate > endDate) {
        endDate = startDate;
        formik.setFieldValue('endDate', endDate);
      }

      dispatch(setDateFilterAction({
        startDate,
        endDate,
      }));
    }
  };

  const updateEndDate = (event: any, data: any) => {
    if (data.value !== formik.values.endDate) {
      let startDate = formik.values ? formik.values : initNonStdFilters.startDate;
      const endDate = data.value;

      formik.setFieldValue('endDate', endDate);

      if (endDate < startDate) {
        startDate = endDate;
        formik.setFieldValue('startDate', startDate);
      }

      dispatch(setDateFilterAction({
        startDate,
        endDate,
      }));
    }
  };

  const resetFilters = () => {
    formik.resetForm();
    formik.handleSubmit();
    dispatch(resetDateFilterAction());
  };

  return (
    <Form className={styles.form}>
      <Grid padded="vertically">
        <Grid.Row columns={1}>
          <Grid.Column>
            <b>{t('hotDesks.room')}</b>
            <Form.Input
              name="room"
              onChange={updateValues}
            />
          </Grid.Column>
        </Grid.Row>
        <Grid.Row columns={2}>
          <Grid.Column {...columnCommonProps}>
            <b>{t('hotDesks.building')}</b>
            <Form.Select
              name="buildingIds"
              clearable
              multiple
              selection
              options={buildingOptions}
              onChange={updateDropdown}
              value={formik.values.buildingIds || ''}
            />
          </Grid.Column>
          <Grid.Column {...columnCommonProps}>
            <b>{t('hotDesks.floor')}</b>
            <Form.Select
              name="floors"
              clearable
              multiple
              selection
              options={floorOptions}
              onChange={updateDropdown}
              value={formik.values.floors || ''}
            />
          </Grid.Column>
        </Grid.Row>
        <Grid.Row columns={2}>
          <Grid.Column {...columnCommonProps}>
            <b>{t('hotDesks.freeSpace')}</b>
            <Form.Group unstackable>
              <Form.Input
                name="freeHotDeskMin"
                onChange={updateValues}
                type="number"
                width="8"
                placeholder={t('common.from')}
                min={0}
                value={formik.values.freeHotDeskMin || ''}
              />
              <Form.Input
                name="freeHotDeskMax"
                onChange={updateValues}
                type="number"
                width="8"
                placeholder={t('common.to')}
                min={0}
                value={formik.values.freeHotDeskMax || ''}
              />
            </Form.Group>
          </Grid.Column>
          <Grid.Column {...columnCommonProps}>
            <Form.Group unstackable className={styles.flexContainer} widths="equal">
              <Datepicker
                name="startDate"
                onChange={updateStartDate}
                value={formik.values.startDate}
                minDate={loggedUserData.isStandardUser() ? initFilters.startDate : initNonStdFilters.startDate}
                maxDate={loggedUserData.isStandardUser() ? initFilters.endDate : initNonStdFilters.endDate}
                label={t('hotDesks.dateFromLabel')}
                clearable={false}
                clearOnSameDateClick={false}
              />
              <Datepicker
                name="endDate"
                onChange={updateEndDate}
                value={formik.values.endDate}
                minDate={loggedUserData.isStandardUser() ? initFilters.startDate : initNonStdFilters.startDate}
                maxDate={loggedUserData.isStandardUser() ? initFilters.endDate : initNonStdFilters.endDate}
                label={t('hotDesks.dateToLabel')}
                clearable={false}
                clearOnSameDateClick={false}
              />
            </Form.Group>
          </Grid.Column>
        </Grid.Row>
        <Grid.Row columns={1} textAlign="right">
          <Grid.Column>
            <Button type="reset" onClick={resetFilters}>{t('common.clearFilters')}</Button>
          </Grid.Column>
        </Grid.Row>
      </Grid>
    </Form>
  );
};
export default Filters;
