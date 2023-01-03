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

import {
  AddCommentArgs,
  CreateArgs,
  CreateUserArgs,
  DownVoteArgs,
  EnvelopeError,
  EnvelopeSuccess,
  LastUserDto,
  PaginatedPostList,
  PostDetailsDto,
  PostListDto,
  UpVoteArgs,
  UserDetailsDto,
} from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class Api<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Post
   * @name PostList
   * @request GET:/api/Post
   */
  postList = (
    query: {
      /** @format int32 */
      Offset: number;
      /** @format int32 */
      Limit: number;
    },
    params: RequestParams = {},
  ) =>
    this.request<PaginatedPostList, EnvelopeError>({
      path: `/api/Post`,
      method: "GET",
      query: query,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Post
   * @name PostCreate
   * @request POST:/api/Post
   */
  postCreate = (data: CreateArgs, params: RequestParams = {}) =>
    this.request<void, EnvelopeError>({
      path: `/api/Post`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags Post
   * @name PostGetLastestList
   * @request GET:/api/Post/GetLastest
   */
  postGetLastestList = (
    query: {
      /** @format int32 */
      Size: number;
    },
    params: RequestParams = {},
  ) =>
    this.request<PostListDto[], EnvelopeError>({
      path: `/api/Post/GetLastest`,
      method: "GET",
      query: query,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Post
   * @name PostDetail
   * @request GET:/api/Post/{id}
   */
  postDetail = (id: number, params: RequestParams = {}) =>
    this.request<PostDetailsDto, EnvelopeError>({
      path: `/api/Post/${id}`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Post
   * @name PostCloseUpdate
   * @request PUT:/api/Post/Close/{id}
   */
  postCloseUpdate = (id: number, params: RequestParams = {}) =>
    this.request<EnvelopeSuccess, EnvelopeError>({
      path: `/api/Post/Close/${id}`,
      method: "PUT",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Post
   * @name PostAddCommentUpdate
   * @request PUT:/api/Post/AddComment/{id}
   */
  postAddCommentUpdate = (id: number, data: AddCommentArgs, params: RequestParams = {}) =>
    this.request<EnvelopeSuccess, EnvelopeError>({
      path: `/api/Post/AddComment/${id}`,
      method: "PUT",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Post
   * @name PostUpVoteUpdate
   * @request PUT:/api/Post/UpVote/{id}
   */
  postUpVoteUpdate = (id: number, data: UpVoteArgs, params: RequestParams = {}) =>
    this.request<EnvelopeSuccess, EnvelopeError>({
      path: `/api/Post/UpVote/${id}`,
      method: "PUT",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Post
   * @name PostDownVoteUpdate
   * @request PUT:/api/Post/DownVote/{id}
   */
  postDownVoteUpdate = (id: number, data: DownVoteArgs, params: RequestParams = {}) =>
    this.request<EnvelopeSuccess, EnvelopeError>({
      path: `/api/Post/DownVote/${id}`,
      method: "PUT",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags User
   * @name UserGetLastList
   * @request GET:/api/User/GetLast
   */
  userGetLastList = (
    query: {
      /** @format int32 */
      Size: number;
    },
    params: RequestParams = {},
  ) =>
    this.request<LastUserDto[], EnvelopeError>({
      path: `/api/User/GetLast`,
      method: "GET",
      query: query,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags User
   * @name UserDetail
   * @request GET:/api/User/{id}
   */
  userDetail = (id: number, params: RequestParams = {}) =>
    this.request<UserDetailsDto, EnvelopeError>({
      path: `/api/User/${id}`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags User
   * @name UserCreate
   * @request POST:/api/User
   */
  userCreate = (data: CreateUserArgs, params: RequestParams = {}) =>
    this.request<EnvelopeSuccess, EnvelopeError>({
      path: `/api/User`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags User
   * @name UserPermaBanUpdate
   * @request PUT:/api/User/PermaBan/{id}
   */
  userPermaBanUpdate = (id: number, params: RequestParams = {}) =>
    this.request<void, EnvelopeError>({
      path: `/api/User/PermaBan/${id}`,
      method: "PUT",
      ...params,
    });
}
