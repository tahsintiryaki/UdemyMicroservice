using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services
{
    public class CategoryService:ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;

        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            //connection string ile bağlantı kuruldu.
            var client = new MongoClient(databaseSettings.ConnectionString);
            //Database'e gidildi.
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            //Tabloya gidildi.
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);

            _mapper = mapper;
        }

        public async Task<Response<List<CategoryDto>>> GetAllAsync()
        {
            var categories = await _categoryCollection.Find(category => true).ToListAsync();

            return Response<List<CategoryDto>>.Success(_mapper.Map<List<CategoryDto>>(categories), 200);
        }

        public async Task<Response<CategoryDto>> CreateCategory(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _categoryCollection.InsertOneAsync(category);
            //işlem başarılı ise statusCode 200 olmalıdır.
            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);
        }

        public async Task<Response<CategoryDto>> GetByIdAsync(string Id)
        {
            var category = await _categoryCollection.Find<Category>(t => t.Id == Id).FirstOrDefaultAsync();

            if (category == null)
            {
                return Response<CategoryDto>.Fail("Category Not Found", 404);
            }
            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);
        }
    }
}
