export interface IProduct {
    Name: string;
    Url: string;
    Category: string;
    Collection: string;
    MRP: number;
    DiscountPrice: number;
    Tags:ITag
  }

  export interface ITag{
    Tag: string[];
  }