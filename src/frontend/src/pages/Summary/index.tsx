import React, { useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector, useDispatch } from 'react-redux';
import { Segment, Grid, Header } from 'semantic-ui-react';
import { AppState } from '../../store';
import { setLoadingAction } from '../../store/global/actions';
import { getSummary } from '../../store/summary/actions';
import AllSummarySection from './components/allSummarySection';
import ContractorSummarySection from './components/contractorSummarySection';
import DesksSection from './components/desksSection';
import FpSummarySection from './components/fpSummarySection';
import OverallSection from './components/overallSection';

const Summary: React.FC = () => {
  const { t } = useTranslation();
  const dispatch = useDispatch();
  const { summary } = useSelector((state: AppState) => state.summary);

  useEffect(() => {
    dispatch(setLoadingAction(true));
    dispatch(getSummary());
  }, []);

  useEffect(() => {
    document.title = `${t('common.tam')} - ${t('summary.header')}`;
  }, [t]);

  return (
    <Segment basic>
      <Grid columns={2} padded="vertically">
        <Grid.Row verticalAlign="middle">
          <Grid.Column>
            <Header as="h1">{t('summary.header')}</Header>
          </Grid.Column>
        </Grid.Row>
      </Grid>

      <OverallSection summary={summary} />
      <DesksSection summary={summary} />
      <FpSummarySection summary={summary} />
      <ContractorSummarySection summary={summary} />
      <AllSummarySection summary={summary} />
    </Segment>
  );
};

export default Summary;
