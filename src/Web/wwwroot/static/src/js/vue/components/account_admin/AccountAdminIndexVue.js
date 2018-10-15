/* global gridMixin */

function AccountAdminIndexVue() {
  Vue.component('paginate', VuejsPaginate)

  new Vue({
    el: '#account-admin-index-vue',

    mixins: [gridMixin],

    data: {
      url: '/api/account',

      // Filtros de búsqueda
      userName: '',
      email: '',
      phoneNumber: '',
      createAt: '',
      lastLogin: '',
      active: 'None',
      emailConfirmed: 'None'
    },

    mounted() {
      $('#datetimepickerCreateAt').datepicker(this.datePickerOptions)
        .on('changeDate', function () {
          this.createAt = $('#datetimepickerCreateAt').val()
        }.bind(this))

      $('#datetimepickerLastLogin').datepicker(this.datePickerOptions)
        .on('changeDate', function () {
          this.lastLogin = $('#datetimepickerLastLogin').val()
        }.bind(this))
    },

    watch: {
      userName(val) {
        this.userName = val
        this._onFilterChange()
      },
      email(val) {
        this.email = val
        this._onFilterChange()
      },
      phoneNumber(val) {
        this.phoneNumber = val
        this._onFilterChange()
      },
      createAt(val) {
        this.createAt = val
        this._onFilterChange()
      },
      lastLogin(val) {
        this.lastLogin = val
        this._onFilterChange()
      },
      active(val) {
        this.active = val
        this._onFilterChange()
      },
      emailConfirmed(val) {
        this.emailConfirmed = val
        this._onFilterChange()
      }
    },

    methods: {
      onFilterReset() {
        this.userName = ''
        this.email = ''
        this.phoneNumber = ''
        this.createAt = ''
        this.lastLogin = ''
        this.active = 'None'
        this.emailConfirmed = 'None'
        this._onFilterChange()
      },

      createLinkToUserDetails(userId) {
        return `/admin/account/edit/${userId}/`
      },

      _getObjectList() {
        const url = `${this.url}/paginate/`
        this.loading = true

        axios({
          method: 'get',
          url: url,
          params: {
            // Paginación
            pageNumber: this.pageNumber,
            pageSize: this.pageSize,
            sortOrder: this.sortOrder,
            orderBy: this.orderBy,

            // Filtros de búsqueda
            userName: this.userName,
            email: this.email,
            phoneNumber: this.phoneNumber,
            createAt: this.createAt,
            lastLogin: this.lastLogin,
            active: this.active,
            emailConfirmed: this.emailConfirmed
          },
        })
          .then((response) => {
            const data = response.data
            this.objectList = data.objectList
            this.totalItems = data.totalItems
            this.totalPages = data.totalPages
            this.loading = false
          })
      }
    }
  })
}
