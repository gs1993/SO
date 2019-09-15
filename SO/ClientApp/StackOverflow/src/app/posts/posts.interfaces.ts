export interface PostList {
    id: number;
    answerCount: number;
    isClosed: boolean;
    commentCount: number;
    creationDate: Date | null;
    score: number;
    title: string | null;
    shortBody: string | null;
    viewCount: number;
}

export interface PostDetails {
    AnswerCount: number | null;
    Body: string | null;
    ClosedDate: Date | null;
    CommentCount: number | null;
    CommunityOwnedDate: Date | null;
    CreationDate: Date;
    FavoriteCount: number | null;
    LastActivityDate: string;
    LastEditDate: string | null;
    LastEditorDisplayName: string | null;
    Score: number;
    Tags: string | null;
    Title: string | null;
    ViewCount: number;
}

export interface Result<T> {
    result: T[];
    errorMessage: string;
    timeGenerated: string;
}
