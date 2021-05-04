export interface CategoriesModel
{
  categories: Category[]
}

export interface Category {
  id: number;
  name: string;
  total: number;
  hour: string;
} 