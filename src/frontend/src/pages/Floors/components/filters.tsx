import React from 'react';
import { Form, Button, Grid, GridColumnProps,
  DropdownItemProps, DropdownProps, InputOnChangeData } from 'semantic-ui-react';
import { useTranslation } from 'react-i18next';
import { useFormik } from 'formik';
import { GRID_COLUMN_COMPUTER, GRID_COLUMN_MOBILE, GRID_COLUMN_TABLET } from '../../../globalConstants';

export type FilterOptions = {
  buildingIds?: string[],
  floors?: number[],
  occupiedDesksMin?: number,
  occupiedDesksMax?: number,
  capacityMin?: number,
  capacityMax?: number,
}

type ComponentProps = {
  buildingOptions: DropdownItemProps[],
  maxFloor: number,
  onFilter: (filters: FilterOptions) => void,
}

const Filters: React.FC<ComponentProps> = (props) => {
  const { t } = useTranslation();
  const floorOptions: DropdownItemProps[] = [];
  for (let i = 0; i <= props.maxFloor; i++) {
    floorOptions.push({ key: i, text: i, value: i });
  }

  const { buildingOptions } = props;

  const formik = useFormik({
    initialValues: {
      buildingIds: Array(0),
      floors: Array(0),
      capacityMin: undefined,
      capacityMax: undefined,
      occupiedDesksMin: undefined,
      occupiedDesksMax: undefined,
    },

    onSubmit: (values) => {
      const valuesDeepCopy = JSON.parse(JSON.stringify(values));
      if (values.buildingIds!.length === 0) {
        buildingOptions.forEach((item) => valuesDeepCopy.buildingIds.push(item.value));
      }

      if (values.floors!.length === 0) {
        floorOptions.forEach((item) => valuesDeepCopy.floors.push(item.value));
      }

      props.onFilter({
        buildingIds: valuesDeepCopy.buildingIds,
        floors: valuesDeepCopy.floors,
        occupiedDesksMin: valuesDeepCopy.occupiedDesksMin,
        occupiedDesksMax: valuesDeepCopy.occupiedDesksMax,
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

  const updateNumericValues = (event: React.ChangeEvent, data: InputOnChangeData) => {
    formik.setFieldValue(data.name, Number(data.value) > 0 ? data.value : 0);

    formik.handleSubmit();
  };

  const resetFilters = () => {
    formik.resetForm();
    formik.handleSubmit();
  };

  return (
    <Form>
      <Grid padded="vertically">
        <Grid.Row columns={2}>
          <Grid.Column {...columnCommonProps}>
            <b>{t('floors.building')}</b>
            <Form.Select
              name="buildingIds"
              multiple
              clearable
              selection
              options={buildingOptions}
              onChange={updateFormik}
              value={formik.values.buildingIds || ''}
            />
          </Grid.Column>
          <Grid.Column {...columnCommonProps}>
            <b>{t('floors.floor')}</b>
            <Form.Select
              name="floors"
              multiple
              clearable
              selection
              options={floorOptions}
              onChange={updateFormik}
              value={formik.values.floors || 0}
            />
          </Grid.Column>
        </Grid.Row>
        <Grid.Row columns={2}>
          <Grid.Column {...columnCommonProps}>
            <b>{t('floors.occupiedDesks')}</b>
            <Form.Group unstackable>
              <Form.Input
                name="occupiedDesksMin"
                onChange={updateNumericValues}
                type="number"
                width="8"
                placeholder={t('common.from')}
                min={0}
                value={formik.values.occupiedDesksMin || ''}
              />
              <Form.Input
                name="occupiedDesksMax"
                onChange={updateNumericValues}
                type="number"
                width="8"
                placeholder={t('common.to')}
                min={0}
                value={formik.values.occupiedDesksMax || ''}
              />
            </Form.Group>
          </Grid.Column>
          <Grid.Column {...columnCommonProps}>
            <b>{t('floors.capacity')}</b>
            <Form.Group unstackable>
              <Form.Input
                name="capacityMin"
                onChange={updateNumericValues}
                type="number"
                width="8"
                placeholder={t('common.from')}
                min={0}
                value={formik.values.capacityMin || ''}
              />
              <Form.Input
                name="capacityMax"
                onChange={updateNumericValues}
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
