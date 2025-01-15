using System.Windows;
using System.Windows.Controls;
using Domain;

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

        private async void AddMemberButton_Click(object sender, RoutedEventArgs e)
        {
            var memberName = MemberNameTextBox.Text;
            var memberEmail = MemberEmailTextBox.Text;
            var memberRole = MemberRoleTextBox.Text;

            // Validation simple
            if (string.IsNullOrWhiteSpace(memberName) || string.IsNullOrWhiteSpace(memberEmail) || string.IsNullOrWhiteSpace(memberRole))
            {
                MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newMember = new Member
            {
                Name = memberName,
                Email = memberEmail,
                Role = memberRole
            };

            await _memberService.AddMemberAsync(newMember);
            LoadMembers();

            // Réinitialisation des champs
            MemberNameTextBox.Text = string.Empty;
            MemberEmailTextBox.Text = string.Empty;
            MemberRoleTextBox.Text = string.Empty;
        }

        private async void DeleteMemberButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.Tag is Member member)
            {
                // Demande de confirmation
                var result = MessageBox.Show($"Êtes-vous sûr de vouloir supprimer {member.Name} ?",
                                             "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    await _memberService.DeleteMemberAsync(member.Id);
                    LoadMembers();
                }
            }
        }
    }
}
