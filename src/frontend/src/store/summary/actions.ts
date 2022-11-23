import { SummaryDto } from '../../services/summary/models';
import { GET_SUMMARY, GET_SUMMARY_SUCCESS, IGetSummary, IGetSummarySuccess } from './types';

export function getSummary(): IGetSummary {
  return {
    type: GET_SUMMARY,
  };
}

export function getSummarySuccess(summary: SummaryDto): IGetSummarySuccess {
  return {
    type: GET_SUMMARY_SUCCESS,
    payload: summary,
  };
}
