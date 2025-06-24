using Dapper;
using Npgsql;
using Otus.SocNet.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus.SocNet.DAL
{
    public class SocialRepository : ISocialRepository
    {
        private readonly string _connectionString;

        public SocialRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private NpgsqlConnection GetConnection() => new(_connectionString);

        public async Task AddFriendAsync(int userId, int friendId)
        {
            var _connection = GetConnection();
            const string sql = "INSERT INTO friends (user_id, friend_id) VALUES (@UserId, @FriendId) ON CONFLICT DO NOTHING";
            await _connection.ExecuteAsync(sql, new { UserId = userId, FriendId = friendId });
        }

        public async Task<int> CreatePostAsync(Post post)
        {
            var _connection = GetConnection();
            const string sql = "INSERT INTO posts (author_id, content,created_at) VALUES (@AuthorId, @Content, @CreatedAt) RETURNING id";
            return await _connection.ExecuteScalarAsync<int>(sql, post);
        }

        public async Task DeleteFriendAsync(int userId, int friendId)
        {
            var _connection = GetConnection();
            const string sql = "DELETE FROM friends WHERE user_id = @UserId AND friend_id = @FriendId";                  
            await _connection.ExecuteAsync(sql, new { UserId = userId, FriendId = friendId });
        }

        public async Task<List<int>> GetFollowersAsync(int authorId)
        {
            var _connection = GetConnection();
            const string sql = "SELECT user_id FROM friends WHERE friend_id = @AuthorId";
            var followers = await _connection.QueryAsync<int>(sql, new { AuthorId = authorId });
            return followers.ToList();
        }

        public async Task<List<Post>> GetFeedAsync(int userId)
        {
            var _connection = GetConnection();
            const string sql = @"
        SELECT p.*
        FROM posts p
        JOIN friends f ON p.author_id = f.friend_id
        WHERE f.user_id = @UserId
        ORDER BY p.created_at DESC
        LIMIT 1000";
            var posts = await _connection.QueryAsync<Post>(sql, new { UserId = userId });
            return posts.ToList();
        }       
    }
}
