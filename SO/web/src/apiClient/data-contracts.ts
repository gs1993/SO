/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export interface AddCommentArgs {
  /** @format int32 */
  userId?: number;
  comment?: string | null;
}

export interface CommentDto {
  /** @format date-time */
  creationDate?: string;
  /** @format int32 */
  score?: number | null;
  text?: string | null;
  userName?: string | null;
}

export interface CreateArgs {
  title?: string | null;
  body?: string | null;
  /** @format int32 */
  authorId?: number;
  tags?: string | null;
  /** @format int32 */
  parentId?: number | null;
}

export interface CreateUserArgs {
  aboutMe?: string | null;
  /** @format int32 */
  age?: number | null;
  displayName?: string | null;
  location?: string | null;
  websiteUrl?: string | null;
}

export interface DownVoteArgs {
  /** @format int32 */
  userId?: number;
}

export interface EnvelopeError {
  errorMessage?: string | null;
  /** @format date-time */
  timeGenerated?: string;
}

export interface EnvelopeSuccess {
  /** @format date-time */
  timeGenerated?: string;
}

export interface LastUserDto {
  /** @format int32 */
  id?: number;
  displayName?: string | null;
}

export interface PaginatedPostList {
  posts?: PostListDto[] | null;
  /** @format int32 */
  count?: number;
}

export interface PostDetailsDto {
  /** @format int32 */
  id?: number;
  /** @format int32 */
  answerCount?: number;
  body?: string | null;
  /** @format date-time */
  closedDate?: string | null;
  /** @format int32 */
  commentCount?: number;
  isClosed?: boolean;
  /** @format date-time */
  communityOwnedDate?: string | null;
  /** @format date-time */
  creationDate?: string;
  /** @format int32 */
  favoriteCount?: number;
  /** @format date-time */
  lastActivityDate?: string;
  /** @format date-time */
  lastEditDate?: string | null;
  lastEditorDisplayName?: string | null;
  /** @format int32 */
  score?: number;
  tags?: string | null;
  title?: string | null;
  /** @format int32 */
  viewCount?: number;
  comments?: CommentDto[] | null;
}

export interface PostListDto {
  /** @format int32 */
  id?: number;
  /** @format int32 */
  answerCount?: number;
  isClosed?: boolean;
  /** @format int32 */
  commentCount?: number;
  /** @format date-time */
  creationDate?: string;
  /** @format int32 */
  score?: number;
  title?: string | null;
  body?: string | null;
  /** @format int32 */
  viewCount?: number;
  tags?: string[] | null;
}

export interface UpVoteArgs {
  /** @format int32 */
  userId?: number;
}

export interface UserDetailsDto {
  /** @format int32 */
  id?: number;
  aboutMe?: string | null;
  /** @format int32 */
  age?: number | null;
  /** @format date-time */
  creationDate?: string;
  displayName?: string | null;
  /** @format date-time */
  lastAccessDate?: string;
  location?: string | null;
  /** @format int32 */
  reputation?: number;
  /** @format int32 */
  views?: number;
  websiteUrl?: string | null;
  /** @format int32 */
  createdPostCount?: number;
  /** @format int32 */
  voteCount?: number;
  /** @format int32 */
  upVotes?: number;
  /** @format int32 */
  downVotes?: number;
}
