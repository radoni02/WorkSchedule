import { createStore } from 'vuex';

const store = createStore({
  state: {
    user: null,
    accessToken: null,
  },
  mutations: {
    setUser(state, user) {
      state.user = user;
    },
    setAccessToken(state, token) {
      state.accessToken = token;
    },
  },
  actions: {
    updateUser({ commit }, user) {
      commit('setUser', user);
    },
    updateAccessToken({ commit }, token) {
      commit('setAccessToken', token);
    },
  },
  getters: {
    getUser: (state) => state.user,
    getAccessToken: (state) => state.accessToken,
  },
});

export default store;
