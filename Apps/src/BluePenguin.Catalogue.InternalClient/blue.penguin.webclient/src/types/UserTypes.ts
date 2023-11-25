export interface IProduct {
    name: string;
    url: string;
    category: string;
    collection: string;
    mrp: number;
    discountPrice: number;
    tags:ITag
  }

  export interface ITag{
    tag: string[];
  }