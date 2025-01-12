using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MemberService
{
    private readonly GenericRepository<Member> _memberRepository;

    public MemberService(GenericRepository<Member> memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<IEnumerable<Member>> GetAllMembersAsync()
    {
        return await _memberRepository.GetAllAsync();
    }

    public async Task<Member> GetMemberByIdAsync(int id)
    {
        return await _memberRepository.GetByIdAsync(id);
    }

    public async Task AddMemberAsync(Member member)
    {
        await _memberRepository.AddAsync(member);
    }

    public async Task UpdateMemberAsync(Member member)
    {
        await _memberRepository.UpdateAsync(member);
    }

    public async Task DeleteMemberAsync(int id)
    {
        var member = await _memberRepository.GetByIdAsync(id);
        if (member != null)
        {
            await _memberRepository.DeleteAsync(member);
        }
    }
}
