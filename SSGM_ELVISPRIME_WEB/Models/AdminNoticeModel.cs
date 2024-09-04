using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSGM_ELVISPRIME_WEB.Models
{
    public class AdminNoticeModel
    {
        public string FLAG { get; set; }
        /// <summary>
        /// 게시판 Key 값
        /// </summary>
        public int NOTICE_ID { get; set; }
        /// <summary>
        /// 공지사항 제목
        /// </summary>
        public string TITLE { get; set; }

        /// <summary>
        /// 공지사항 구분
        /// </summary>
        public string TYPE { get; set; }
        /// <summary>
        /// 조회수
        /// </summary>
        public int CNT { get; set; }
        /// <summary>
        /// 글쓴이
        /// </summary>
        public string WRITER { get; set; }
        /// <summary>
        /// 사용여부
        /// </summary>
        public string USE_YN { get; set; }
        /// <summary>
        /// 공지여부
        /// </summary>
        public string NOTICE_YN { get; set; }
        /// <summary>
        /// 등록일자
        /// </summary>
        public string REGDT { get; set; }
        /// <summary>
        /// 수정일자
        /// </summary>
        public string EDITDT { get; set; }
        /// <summary>
        /// 첨부파일1 (물리적파일명)
        /// </summary>
        /// 첨부파일1 (물리적파일명)
        /// </summary>
        public string FILE { get; set; }        
        public string FILE_NAME { get; set; }        
        public string FILE1 { get; set; }        
        public string FILE_NAME1 { get; set; }        
        public string FILE2 { get; set; }
        public string FILE_NAME2 { get; set; }
        
        public HttpPostedFileBase infile { get; set; }
        
        public HttpPostedFileBase infile1 { get; set; }
        
        public HttpPostedFileBase infile2 { get; set; }
        
        /// <summary>
        /// 내용 (Editor사용)
        /// </summary>
        public string CONTENT { get; set; }
    }
}