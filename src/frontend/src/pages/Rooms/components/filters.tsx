import React from 'react';
import { Form, Button, Grid, GridColumnProps,
  DropdownItemProps, DropdownProps, InputOnChangeData } from 'semantic-ui-react';
import { useTranslation } from 'react-i18next';
import { useFormik } from 'formik';
import 'react-semantic-ui-datepickers/dist/react-semantic-ui-datepickers.css';
import './filter.module.scss';
import { GRID_COLUMN_COMPUTER, GRID_COLUMN_MOBILE, GRID_COLUMN_TABLET } from '../../../globalConstants';

export type FilterOptions = {
  room?: string,
  buildingIds?: string[],
  floors?: number[],
  freeSpaceMin?: number,
  freeSpaceMax?: number,
  capacityMin?: number,
  capacityMax?: number,
}

type ComponentProps = {
  buildingOptions: DropdownItemProps[],
  maxFloor: number,
  onFilter: (filters: FilterOptions) => void,
}

const Filters: React.FC<ComponentProps> = (props) => {
  const { buildingOptions } = props;
  const floorOptions: DropdownItemProps[] = [];
  for (let i = 0; i <= props.maxFloor; i++) {
    floorOptions.push({ key: i, text: i, value: i });
  }
  const { t } = useTranslation();
  const formik = useFormik({
    initialValues: {
      room: '',
      buildingIds: Array(0),
      floors: Array(0),
      freeSpaceMin: undefined,
      freeSpaceMax: undefined,
      capacityMin: undefined,
      capacityMax: undefined,
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
        room: valuesDeepCopy.room,
        buildingIds: valuesDeepCopy.buildingIds,
        floors: valuesDeepCopy.floors,
        freeSpaceMin: valuesDeepCopy.freeSpaceMin,
        freeSpaceMax: valuesDeepCopy.freeSpaceMax,
        capacityMin: valuesDeepCopy.capacityMin,
        capacityMax: valuesDeepCopy.capacityMax,
      });
    },
  });

  const columnCommonProps: GridColumnProps = {
    mobile: GRID_COLUMN_MOBILE, tablet: GRID_COLUMN_TABLET, computer: GRID_COLUMN_COMPUTER };

  const updateFormik = (event: React.SyntheticEvent<HTMLElement>, data: DropdownProps) => {
    formik.setFieldValue(data.name, data.value);
    formik.handleSubmit();
  };

  const updateValues = (event: React.ChangeEvent, data: InputOnChangeData) => {
    if (data.type === 'number') {
      formik.setFieldValue(data.name, Number(data.value) > 0 ? data.value : 0);
    } else {
      formik.setFieldValue(data.name, data.value);
    }
    formik.handleSubmit();
  };

  const resetFilters = () => {
    formik.resetForm();
    formik.handleSubmit();
  };

  return (
    <Form>
      <Grid padded="vertically">
        <Grid.Row columns={1}>
          <Grid.Column>
            <b>{t('rooms.room')}</b>
            <Form.Input
              name="room"
              onChange={updateValues}
            />
          </Grid.Column>
        </Grid.Row>
        <Grid.Row columns={2}>
          <Grid.Column {...columnCommonProps}>
            <b>{t('rooms.building')}</b>
            <Form.Select
              name="buildingIds"
              clearable
              multiple
              selection
              options={buildingOptions}
              onChange={updateFormik}
              value={formik.values.buildingIds || ''}
            />
          </Grid.Column>
          <Grid.Column {...columnCommonProps}>
            <b>{t('rooms.floor')}</b>
            <Form.Select
              name="floors"
              clearable
              multiple
              selection
              options={floorOptions}
              onChange={updateFormik}
              value={formik.values.floors || ''}
            />
          </Grid.Column>
        </Grid.Row>
        <Grid.Row columns={2}>
          <Grid.Column {...columnCommonProps}>
            <b>{t('rooms.freeSpace')}</b>
            <Form.Group unstackable>
              <Form.Input
                name="freeSpaceMin"
                onChange={updateValues}
                type="number"
                width="8"
                placeholder={t('common.from')}
                min={0}
                value={formik.values.freeSpaceMin || ''}
              />
              <Form.Input
                name="freeSpaceMax"
                onChange={updateValues}
                type="number"
                width="8"
                placeholder={t('common.to')}
                min={0}
                value={formik.values.freeSpaceMax || ''}
              />
            </Form.Group>
          </Grid.Column>
          <Grid.Column {...columnCommonProps}>
            <b>{t('rooms.size')}</b>
            <Form.Group unstackable>
              <Form.Input
                name="capacityMin"
                onChange={updateValues}
                type="number"
                width="8"
                placeholder={t('common.from')}
                min={0}
                value={formik.values.capacityMin || ''}
              />
              <Form.Input
                name="capacityMax"
                onChange={updateValues}
                type="number"
                width="8"
                placeholder={t('common.to')}
                min={0}
                value={formik.values.capacityMax || ''}
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
