import { PerformanceModel } from "./PerformanceModel";

export interface PerformanceApiModel {
    startDate : Date,
    endDate : Date,
    resultCount : number,
    models : PerformanceModel[]
}