import { FinancialProperty } from "./FinancialProperty";

export interface Financial {
    symbol : string,
    fromDate : string,
    toDate: string,
    year: string,
    properties: FinancialProperty[],
}