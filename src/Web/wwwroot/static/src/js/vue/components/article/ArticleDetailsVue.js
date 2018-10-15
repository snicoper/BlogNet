function ArticleDetailsVue() {
  new Vue({
    el: '#article-details-vue',

    data: {
      url: '/api/article',
      id: 0,
      likes: 0,
      canVoteLike: true,
      canVoteLikeName: ''
    },

    mounted() {
      this.likes = $('#article-likes').data('likes')
      this.id = $('#article-details-vue').data('article-id')
      this.canVoteLikeName = `article-can-vote-${this.id}`

      this.canVoteLike = !localStorage.getItem(this.canVoteLikeName)
    },

    methods: {
      onIncrease() {
        const url = `${this.url}/increase/like/${this.id}`

        if (this.canVoteLike === true) {
          axios({
            url: url,
            method: 'put'
          })
            .then((response) => {
              this.likes = response.data.likes
              this.canVoteLike = false
              localStorage.setItem(this.canVoteLikeName, 'false')
              toastr.success('Gracias por votar')
            })
        } else {
          toastr.warning('Ya has votado en este articulo')
        }
      }
    }
  })
}
