using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace myTasker.Views
{
    public partial class MemberManagement : UserControl
    {
        private readonly MemberService _memberService;

        public MemberManagement(MemberService memberService)
        {
            InitializeComponent();
            _memberService = memberService;
            LoadMembers();
        }

        private async void LoadMembers()
        {
            var members = await _memberService.GetAllMembersAsync();
            MembersListView.ItemsSource = members;
        }

        private void AddMemberButton_Click(object sender, RoutedEventArgs e)
        {
            var memberName = MemberNameTextBox.Text;
            var memberEmail = MemberEmailTextBox.Text;
            var memberRole = MemberRoleTextBox.Text;

            // Vous pouvez ajouter ici une validation des champs si nécessaire

            var newMember = new Member
            {
                Name = memberName,
                Email = memberEmail,
                Role = memberRole
            };

            _ = _memberService.AddMemberAsync(newMember);
            LoadMembers();
        }
    }
}
