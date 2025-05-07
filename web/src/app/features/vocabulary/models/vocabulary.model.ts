export interface VocabularyMeaningDTO {
  id: number;
  vocabularyId: number;
  partOfSpeech: string;
  meaning: string;
  ipa: string;
  pronunciation: string;
  example: string;
  note: string;
  usage: string;
}

export interface VocabularyDTO {
  id: number;
  word: string;
  userId: number;
  meanings: VocabularyMeaningDTO[];
}

export interface CreateVocabularyRequest {
  word: string;
  meanings: {
    partOfSpeech: string;
    meaning: string;
    ipa: string;
    pronunciation: string;
    example: string;
    note: string;
    usage: string;
  }[];
}

export interface UpdateVocabularyRequest {
  id: number;
  word: string;
  meanings: {
    id: number;
    partOfSpeech: string;
    meaning: string;
    ipa: string;
    pronunciation: string;
    example: string;
    note: string;
    usage: string;
  }[];
}
