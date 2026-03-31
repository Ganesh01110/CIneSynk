import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

export const apiSlice = createApi({
  reducerPath: 'api',
  baseQuery: fetchBaseQuery({
    baseUrl: '/api',
    prepareHeaders: (headers, { getState }) => {
      // Get token from auth state (once implemented)
      const token = (getState() as any).auth?.token;
      if (token) {
        headers.set('authorization', `Bearer ${token}`);
      }
      return headers;
    },
  }),
  tagTypes: ['Show', 'Booking', 'User'],
  endpoints: (builder) => ({
    login: builder.mutation({
      query: (credentials) => ({
        url: '/auth/login',
        method: 'POST',
        body: credentials,
      }),
    }),
    register: builder.mutation({
      query: (userData) => ({
        url: '/auth/register',
        method: 'POST',
        body: userData,
      }),
    }),
    getShows: builder.query<any[], void>({
      query: () => '/shows',
      providesTags: ['Show'],
    }),
    getShowSeats: builder.query<any, number>({
      query: (showId) => `/shows/${showId}/seats`,
      providesTags: (result, error, id) => [{ type: 'Show', id }],
    }),
    reserveSeat: builder.mutation({
      query: ({ showId, seatId }) => ({
        url: `/bookings/reserve/${showId}/${seatId}`,
        method: 'POST',
        // Note: X-Idempotency-Key would be added here or in middleware
      }),
      invalidatesTags: ['Show'],
    }),
  }),
});

export const { 
  useLoginMutation, 
  useRegisterMutation, 
  useGetShowsQuery,
  useGetShowSeatsQuery,
  useReserveSeatMutation 
} = apiSlice;
