import React from 'react';
import {
  Form,
  Button,
  Grid,
  GridColumnProps,
} from 'semantic-ui-react';
import { useTranslation } from 'react-i18next';
import { GRID_COLUMN_COMPUTER, GRID_COLUMN_MOBILE, GRID_COLUMN_TABLET } from '../../../globalConstants';
import { ProjectsFilterOptions } from '../../../services/project/models';

type ComponentProps = {
    onFilter: (filtersArgs: ProjectsFilterOptions) => void
    filtersState: ProjectsFilterOptions
}

const Filters: React.FC<ComponentProps> = ({ onFilter, filtersState }) => {
  const { t } = useTranslation();

  const columnCommonProps: GridColumnProps = {
    mobile: GRID_COLUMN_MOBILE, tablet: GRID_COLUMN_TABLET, computer: GRID_COLUMN_COMPUTER,
  };

  const resetFilters = () => {
    onFilter({
      externalCompanies: false,
    });
  };

  const handleClick = (_: any) => {
  };

  return (
    <Form>
      <Grid padded="vertically">
        <Grid.Row columns={2}>
          <Grid.Column {...columnCommonProps}>
            <button type="button" onClick={() => handleClick(filtersState)}>Dummy filter!!!</button>
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
