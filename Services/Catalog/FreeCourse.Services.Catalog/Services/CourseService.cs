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
    internal class CourseService:ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;

        private readonly IMapper _mapper;

        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings)
        {

            //connection string ile bağlantı kuruldu.
            var client = new MongoClient(databaseSettings.ConnectionString);
            //Database'e gidildi.
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            //Tabloya gidildi.
            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);

            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);


            _mapper = mapper;
        }


        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(course => true).ToListAsync();



            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find<Category>(t => t.Id == course.CategoryId).FirstOrDefaultAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }

            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Response<CourseDto>> GetByIdAsync(string Id)
        {
            var course = await _courseCollection.Find<Course>(t => t.Id == Id).FirstOrDefaultAsync();

            if (course == null)
            {
                return Response<CourseDto>.Fail("Course Not Found", 404);
            }

            course.Category = await _categoryCollection.Find<Category>(t => t.Id == course.CategoryId).FirstOrDefaultAsync();

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
        }

        public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find<Course>(t => t.UserId == userId).ToListAsync();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find<Category>(t => t.Id == course.CategoryId).FirstOrDefaultAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }

            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
        {
            var newCourse = _mapper.Map<Course>(courseCreateDto);
            newCourse.CreatedTime = DateTime.Now;

            await _courseCollection.InsertOneAsync(newCourse);

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);

        }

        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var updateCourse = _mapper.Map<Course>(courseUpdateDto);

            //Buradaki yapı course tablosuna git.
            //FindOneAndReplaceAsync => bull ve değiştir.
            //courseUpdateDto daki Id ile courseleri ara
            //eğer kayıt varsa git db de yakalanan "updateCourse" kaydını güncelle.
            //Eğer kayıt yok ise result null olacaktır.
            var result = await _courseCollection.FindOneAndReplaceAsync(t => t.Id == courseUpdateDto.Id, updateCourse);
            if (result == null)
            {
                return Response<NoContent>.Fail("Couse Not Found", 404);
            }

            //Güncelleme de 204 yani body'si olmayan sadece başarılı durum kodu  dönülür..
            return Response<NoContent>.Success(204);
        }

        public async Task<Response<NoContent>> DeleteAsync(string Id)
        {
            var result = await _courseCollection.DeleteOneAsync(t => t.Id == Id);
            if (result.DeletedCount > 0)
            {
                return Response<NoContent>.Success(204);
            }

            return Response<NoContent>.Fail("Course Not Found", 404);

        }
    }
}
