import { createStore } from 'vuex';

const store = createStore({
  state: {
    user: null,
  },
  mutations: {
    setUser(state, user) {
      state.user = user;
    },
  },
  actions: {
    updateUser({ commit }, user) {
      commit('setUser', user);
    },
  },
  getters: {
    getUser: (state) => state.user,
  },
});

export default store;
