interface SummaryProfile {
  address1: string;
  city: string;
  state: string;
  country: string;
  website: string;
  industry: string;
  sector: string;
  longBusinessSummary: string;
}

interface HistoricalDataPrice {
  date: number;
  open: number;
  high: number;
  low: number;
  close: number;
  volume: number;
  adjustedClose: number;
}

export interface CompanyInfo {
  longName: string;
  symbol: string;
  usedInterval: string;
  usedRange: string;
  logoUrl: string;
  summaryProfile: SummaryProfile;
  historicalDataPrice: HistoricalDataPrice[];
}
