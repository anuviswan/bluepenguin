
export interface KeyValuePair{
    Id:string;
    Name:string;
}
export interface Category extends KeyValuePair{}

export interface Material extends KeyValuePair{}

export interface Feature extends KeyValuePair{}

export interface Collection extends KeyValuePair{}

export interface Product{
    Name : string,
    Category : string,
    Material : string,
    FeatureCodes : Array<string>,
    CollectionCode: string,
    YearCode : string,
}