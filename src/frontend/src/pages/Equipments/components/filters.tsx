import React from 'react';
import {
  Form,
  Button,
  Grid,
  InputOnChangeData,
} from 'semantic-ui-react';
import { useTranslation } from 'react-i18next';
import { useFormik } from 'formik';
import { debounce } from 'lodash';
import { EquipmentsFilterOptions } from '../../../services/equipments/models';

type ComponentProps = {
  onFilter: (filtersArgs: EquipmentsFilterOptions) => void;
};

const Filters: React.FC<ComponentProps> = ({ onFilter }) => {
  const { t } = useTranslation();

  const formik = useFormik({
    initialValues: {
      name: '',
    },

    onSubmit: (values) => {
      onFilter({
        ...values,
      });
    },
  });

  const updateNameFilter = debounce((_: React.ChangeEvent, data: InputOnChangeData) => {
    formik.setFieldValue(data.name, data.value);
    formik.handleSubmit();
  }, 500);

  const resetFilters = () => {
    formik.resetForm();
    formik.handleSubmit();
  };

  return (
    <Form>
      <Grid padded="vertically">
        <Grid.Row columns={1}>
          <Grid.Column>
            <b>{t('equipments.name')}</b>
            <Form.Input name="name" onChange={updateNameFilter} />
          </Grid.Column>
        </Grid.Row>
        <Grid.Row columns={1} textAlign="right">
          <Grid.Column>
            <Button type="reset" onClick={resetFilters}>
              {t('common.clearFilters')}
            </Button>
          </Grid.Column>
        </Grid.Row>
      </Grid>
    </Form>
  );
};
export default Filters;
