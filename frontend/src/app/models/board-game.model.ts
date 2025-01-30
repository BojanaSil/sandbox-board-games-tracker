export interface BoardGame{
  id: string;
  name: string;
  releaseDate: number;
  averagePlayingTimeInMinutes: number;
  minNumberOfPlayers: number;
  maxNumberOfPlayers: number;
  minimumAge: number;
  difficulty: number;
  categories: string;
}
